using MediatR;

namespace OnAim.Admin.APP.Commands.EndPoint.Delete
{
    public record DeleteEndpointCommand(int Id) : IRequest;
}
