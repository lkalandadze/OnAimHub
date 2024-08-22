using MediatR;
using OnAim.Admin.Infrasturcture.Models.Request.EndpointGroup;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Create
{
    public class CreateEndpointGroupCommand : IRequest<ApplicationResult>
    {
        public CreateEndpointGroupRequest Model { get; set; }
    }
}
