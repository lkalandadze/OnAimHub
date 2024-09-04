using OnAim.Admin.APP.Commands.Abstract;
using OnAim.Admin.Infrasturcture.Models.Request.Role;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.Role.Create
{
    public record CreateRoleCommand(CreateRoleRequest Request) : ICommand<ApplicationResult>;
}
