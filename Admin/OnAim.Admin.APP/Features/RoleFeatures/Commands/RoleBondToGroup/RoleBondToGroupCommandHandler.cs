using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.RoleBondToGroup;

public class RoleBondToGroupCommandHandler : ICommandHandler<RoleBondToGroupCommand, ApplicationResult>
{
    private readonly IConfigurationRepository<RoleEndpointGroup> _configurationRepository;
    private readonly IConfigurationRepository<UserRole> _userRoleRepository;

    public RoleBondToGroupCommandHandler(
        IConfigurationRepository<RoleEndpointGroup> configurationRepository,
        IConfigurationRepository<UserRole> userRoleRepository
        )
    {
        _configurationRepository = configurationRepository;
        _userRoleRepository = userRoleRepository;
    }
    public async Task<ApplicationResult> Handle(RoleBondToGroupCommand request, CancellationToken cancellationToken)
    {
        if (request.Groups != null)
        {
            foreach (var group in request.Groups)
            {
                var roleGroup = await _configurationRepository.Query(ur => ur.RoleId == request.RoleId && ur.EndpointGroup.Id == group.Id)
                           .FirstOrDefaultAsync();

                if (roleGroup == null)
                {
                    throw new BadRequestException("Bad Request.");
                }

                roleGroup.IsActive = group.IsActive;

                await _configurationRepository.CommitChanges();
            }
        }

        if (request.Users != null)
        {
            foreach (var user in request.Users)
            {
                var roleUser = await _userRoleRepository.Query(ur => ur.RoleId == request.RoleId && ur.User.Id == user.Id)
                           .FirstOrDefaultAsync();

                if (roleUser == null)
                {
                    throw new BadRequestException("Bad Request.");
                }

                roleUser.IsActive = user.IsActive;

                await _userRoleRepository.CommitChanges();
            }
        }

        return new ApplicationResult { Success = true };
    }
}
