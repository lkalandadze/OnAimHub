using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Infrasturcture.Models.Request.Role;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.Role.Update
{
    public record UpdateRoleCommand(int Id, UpdateRoleRequest Model) : ICommand<ApplicationResult>;
}
