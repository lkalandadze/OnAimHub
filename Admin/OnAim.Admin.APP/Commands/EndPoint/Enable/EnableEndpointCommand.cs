using MediatR;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndPoint.Enable
{
    public record EnableEndpointCommand(int EndpointId) : IRequest<ApplicationResult>;
}
