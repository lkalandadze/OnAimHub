using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.UserBondToRole;

public class UserBondToRoleCommandHandler
    : ICommandHandler<UserBondToRoleCommand, ApplicationResult>
{
    private readonly IConfigurationRepository<UserRole> _repository;

    public UserBondToRoleCommandHandler(IConfigurationRepository<UserRole> repository)
    {
        _repository = repository;
    }
    public async Task<ApplicationResult> Handle(UserBondToRoleCommand request, CancellationToken cancellationToken)
    {
        if (request.Roles == null) throw new BadRequestException("Bad Request");

        foreach (var item in request.Roles)
        {
            var userRole = await _repository.Query(ur => ur.UserId == request.UserId && ur.RoleId == item.Id)
                                        .FirstOrDefaultAsync();

            if (userRole == null)
                throw new NotFoundException("User Role not found.");

            userRole.IsActive = item.IsActive ?? true;

            await _repository.CommitChanges();
        }

        return new ApplicationResult { Success = true };
    }
}
