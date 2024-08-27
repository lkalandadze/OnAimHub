using MediatR;
using OnAim.Admin.Infrasturcture.Models.Request.EndpointGroup;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Update
{
    public record UpdateEndpointGroupCommand(int Id, UpdateEndpointGroupRequest model) : IRequest<ApplicationResult>;
}
