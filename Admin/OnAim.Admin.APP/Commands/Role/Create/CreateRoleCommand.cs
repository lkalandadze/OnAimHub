using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Role;

namespace OnAim.Admin.APP.Commands.Role.Create
{
    public record CreateRoleCommand(CreateRoleRequest Request) : ICommand<ApplicationResult>;
}
