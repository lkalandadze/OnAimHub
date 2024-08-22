using MediatR;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndPoint.Delete
{
    public record DeleteEndpointCommand(int Id) : IRequest<ApplicationResult>;
}
