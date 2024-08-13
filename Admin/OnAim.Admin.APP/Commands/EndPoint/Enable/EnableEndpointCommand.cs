using MediatR;
using OnAim.Admin.APP.Models;

namespace OnAim.Admin.APP.Commands.EndPoint.Enable
{
    public record EnableEndpointCommand(string EndpointId) : IRequest<ApplicationResult>;
}
