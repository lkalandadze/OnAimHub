using FluentValidation;
using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Commands.EndPoint.Delete
{
    public class DeleteEndpointCommandHandler : IRequestHandler<DeleteEndpointCommand>
    {
        private readonly IEndpointRepository _endpointRepository;
        private readonly IValidator<DeleteEndpointCommand> _validator;

        public DeleteEndpointCommandHandler(
            IEndpointRepository endpointRepository, 
            IValidator<DeleteEndpointCommand> validator
            )
        {
            _endpointRepository = endpointRepository;
            _validator = validator;
        }
        public Task Handle(DeleteEndpointCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
