using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Auth;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.APP.Exceptions;
using OnAim.Admin.APP.Models.Response.User;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Identity.Services;
using OnAim.Admin.Infrasturcture.Configuration;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Infrasturcture.Models.Response.EndpointGroup;
using OnAim.Admin.Infrasturcture.Models.Response.Role;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.Models;
using System.Security.Claims;

namespace OnAim.Admin.APP.Commands.User.Login
{
    public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, AuthResultDto>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _userRepository;
        private readonly IRepository<Infrasturcture.Entities.Role> _roleRepository;
        private readonly IConfigurationRepository<UserRole> _userConfigurationRepository;
        private readonly IJwtFactory _jwtFactory;
        private readonly IValidator<LoginUserCommand> _validator;
        private readonly ApplicationUserManager _userManager;
        private readonly IAuditLogService _auditLogService;
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public LoginUserCommandHandler(
            IRepository<Infrasturcture.Entities.User> userRepository,
            IRepository<Infrasturcture.Entities.Role> roleRepository,
            IConfigurationRepository<UserRole> userConfigurationRepository,
            IJwtFactory jwtFactory,
            IValidator<LoginUserCommand> validator,
            ApplicationUserManager userManager,
            IAuditLogService auditLogService,
            ISecurityContextAccessor securityContextAccessor
            )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userConfigurationRepository = userConfigurationRepository;
            _jwtFactory = jwtFactory;
            _validator = validator;
            _userManager = userManager;
            _auditLogService = auditLogService;
            _securityContextAccessor = securityContextAccessor;
        }
        public async Task<AuthResultDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

            var model = request.Model;
            var user = await _userRepository.Query(x => x.Email == model.Email).FirstOrDefaultAsync();

            if (!user.IsActive || user == null)
            {
                throw new UserNotFoundException("User Not Found");
            }

            string hashed = Infrasturcture.Extensions.EncryptPasswordExtension.EncryptPassword(model.Password, user.Salt);

            if (hashed == user.Password)
            {
                var roles = await GetUserRolesAsync(user.Id);

                var permissions = await GetUserPermissionsAsync(user.Id);

                var roleNames = roles.Select(r => r.Name).ToList();
                var token = _jwtFactory.GenerateEncodedToken(user.Id, user.Email, new List<Claim>(), roleNames);

                return new AuthResultDto
                {
                    Token = token,
                    StatusCode = 200
                };
            }

            await _auditLogService.LogEventAsync(
               SystemDate.Now,
               "Login",
               nameof(User),
               user.Id,
               _securityContextAccessor.UserId,
               $"User Logined successfully with ID: {user.Id}");

            return new AuthResultDto
            {
                Token = null,
                StatusCode = 404
            };
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
            var roles = await _userConfigurationRepository
                                      .Query(ur => ur.UserId == userId)
                                      .Select(ur => ur.Role)
                                      .ToListAsync();

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
                        Console.WriteLine("Null EndpointGroup detected for Role: " + role.Name);
                        continue;
                    }

                    foreach (var ege in reg.EndpointGroup.EndpointGroupEndpoints)
                    {
                        if (ege.Endpoint == null)
                        {
                            Console.WriteLine("Null Endpoint detected in EndpointGroup: " + reg.EndpointGroup.Name);
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

}
