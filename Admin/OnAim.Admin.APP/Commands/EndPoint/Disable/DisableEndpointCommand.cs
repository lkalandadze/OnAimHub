using MediatR;
using OnAim.Admin.APP.Models;

namespace OnAim.Admin.APP.Commands.EndPoint.Disable
{
    public record DisableEndpointCommand(string EndpointId) : IRequest<ApplicationResult>;
}
