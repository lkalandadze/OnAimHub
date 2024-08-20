using MediatR;
using OnAim.Admin.Infrasturcture.Models.Request.Role;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.Role.Update
{
    public record UpdateRoleCommand(int Id, UpdateRoleRequest Model) : IRequest<ApplicationResult>; 
}
