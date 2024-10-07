using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure.Configuration;
using OnAim.Admin.Shared.DTOs.Endpoint;
using OnAim.Admin.Shared.DTOs.EndpointGroup;
using OnAim.Admin.Shared.DTOs.Role;
using OnAim.Admin.Shared.DTOs.User;
using OnAim.Admin.Shared.Models;
using System.Security.Claims;

namespace OnAim.Admin.APP.Features.UserFeatures.Commands.TwoFA;

public class VerifyOtpCommandHandler : BaseCommandHandler<VerifyOtpCommand, AuthResultDto>
{
    private readonly IRepository<User> _repository;
    private readonly IJwtFactory _jwtFactory;
    private readonly IOtpService _otpService;
    private readonly IConfigurationRepository<UserRole> _userConfigurationRepository;
    private readonly IRepository<Role> _roleRepository;

    public VerifyOtpCommandHandler(CommandContext<VerifyOtpCommand> context, 
        IRepository<Domain.Entities.User> repository,
         IJwtFactory jwtFactory,
        IOtpService otpService,
        IConfigurationRepository<UserRole> userConfigurationRepository,
        IRepository<Role> roleRepository
        ) : base(context)
    {
        _repository = repository;
        _jwtFactory = jwtFactory;
        _otpService = otpService;
        _userConfigurationRepository = userConfigurationRepository;
        _roleRepository = roleRepository;
    }

    protected async override Task<AuthResultDto> ExecuteAsync(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        var userId = await _repository.Query(x => x.Email == request.Email && x.IsDeleted == false).FirstOrDefaultAsync();
        var storedOtp = await _otpService.GetStoredOtp(userId.Id);
        if (storedOtp == request.OtpCode)
        {
            var user = await _repository.Query(u => u.Id == userId.Id).FirstOrDefaultAsync();
            if (user == null)
                throw new NotFoundException("User doesn't exist");

            var roles = await GetUserRolesAsync(user.Id);
            var roleNames = roles.Select(r => r.Name).ToList();

            var token = _jwtFactory.GenerateEncodedToken(user.Id, user.Email, new List<Claim>(), roleNames);
            var refreshToken = await _jwtFactory.GenerateRefreshToken(user.Id);

            await _jwtFactory.SaveAccessToken(user.Id, token, DateTime.UtcNow.AddDays(1));

            user.LastLogin = SystemDate.Now;

            await _repository.CommitChanges();

            return new AuthResultDto
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                StatusCode = 200
            };
        }

        throw new UnauthorizedAccessException("Invalid OTP");
    }

    private async Task<List<RoleResponseModel>> GetUserRolesAsync(int userId)
    {
        var result = await _userConfigurationRepository.Query(ur => ur.UserId == userId)
                                      .Include(x => x.Role)
                                      .ThenInclude(x => x.RoleEndpointGroups)
                                      .ThenInclude(x => x.EndpointGroup)
                                      .ThenInclude(x => x.EndpointGroupEndpoints)
                                      .ThenInclude(x => x.Endpoint)
                                      .ToListAsync();

        var roles = result
            .Select(x => x.Role)
            .Distinct()
            .OrderBy(role => role.Id)
            .Select(role => new RoleResponseModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                EndpointGroupModels = role.RoleEndpointGroups.Select(z => new EndpointGroupModel
                {
                    Id = z.EndpointGroupId,
                    Name = z.EndpointGroup.Name,
                    Description = z.EndpointGroup.Description,
                    Endpoints = z.EndpointGroup.EndpointGroupEndpoints.Select(u => new EndpointRequestModel
                    {
                        Id = u.Endpoint.Id,
                        Name = u.Endpoint.Name,
                        Description = u.Endpoint.Description,

                    }).ToList()
                }).ToList()
            }).ToList();

        return roles;
    }

    private async Task<IEnumerable<string>> GetUserPermissionsAsync(int userId)
    {
        var roles = await _userConfigurationRepository.Query(ur => ur.UserId == userId).Select(ur => ur.Role).ToListAsync();

        foreach (var role in roles)
        {
            var rolesss = await _roleRepository.Query(x => x.Id == role.Id)
                .Include(x => x.RoleEndpointGroups)
                .ThenInclude(x => x.EndpointGroup)
                .ThenInclude(x => x.EndpointGroupEndpoints)
                .ThenInclude(x => x.Endpoint)
                .SingleOrDefaultAsync();

            foreach (var reg in rolesss.RoleEndpointGroups)
            {
                if (reg.EndpointGroup == null)
                {
                    continue;
                }

                foreach (var ege in reg.EndpointGroup.EndpointGroupEndpoints)
                {
                    if (ege.Endpoint == null)
                    {
                    }
                }
            }

        }
        var permissions = roles
                .Where(role => role != null)
                .SelectMany(role => role.RoleEndpointGroups)
                .Where(reg => reg.EndpointGroup != null)
                .SelectMany(reg => reg.EndpointGroup.EndpointGroupEndpoints)
                .Where(ege => ege.Endpoint != null && ege.Endpoint.IsDeleted)
                .Select(ege => ege.Endpoint.Path)
                .Distinct()
                .ToList();

        return permissions;
    }
}
