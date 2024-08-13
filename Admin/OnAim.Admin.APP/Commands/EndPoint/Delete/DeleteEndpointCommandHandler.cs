using MediatR;

namespace OnAim.Admin.APP.Commands.EndPoint.Delete
{
    public class DeleteEndpointCommandHandler : IRequestHandler<DeleteEndpointCommand>
    {
        public DeleteEndpointCommandHandler()
        {

        }
        public Task Handle(DeleteEndpointCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
