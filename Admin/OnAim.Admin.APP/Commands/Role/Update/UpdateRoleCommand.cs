using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Models.Request.Role;

namespace OnAim.Admin.APP.Commands.Role.Update
{
    public record UpdateRoleCommand(string Id, UpdateRoleRequest Model) : IRequest<ApplicationResult>;
}
