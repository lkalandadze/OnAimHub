using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.Exceptions;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using static OnAim.Admin.Shared.Exceptions.Exceptions;

namespace OnAim.Admin.APP.Commands.User.UserBondToRole
{
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
                {
                    throw new RoleNotFoundException("User Role not found.");
                }

                userRole.IsActive = item.IsActive ?? true;

                await _repository.CommitChanges();
            }

            return new ApplicationResult { Success = true };
        }
    }
}
