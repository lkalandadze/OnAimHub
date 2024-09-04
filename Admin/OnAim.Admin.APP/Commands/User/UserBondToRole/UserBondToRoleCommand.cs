using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.APP.Models.Response.User;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.UserBondToRole
{
    public record UserBondToRoleCommand(int UserId, List<RoleDto>? Roles) : ICommand<ApplicationResult>;
}
