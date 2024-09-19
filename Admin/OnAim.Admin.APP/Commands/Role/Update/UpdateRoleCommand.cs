using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Role;

namespace OnAim.Admin.APP.Commands.Role.Update
{
    public record UpdateRoleCommand(int Id, UpdateRoleRequest Model) : ICommand<ApplicationResult>;
}
