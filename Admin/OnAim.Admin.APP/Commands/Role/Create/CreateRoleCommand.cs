using MediatR;
using OnAim.Admin.Infrasturcture.Models.Request.Role;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.Role.Create
{
    public class CreateRoleCommand : IRequest<ApplicationResult>
    {
        public CreateRoleRequest request { get; set; }
    }
}
