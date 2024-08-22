using FluentValidation;
using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndPoint.Delete
{
    public class DeleteEndpointCommandHandler : IRequestHandler<DeleteEndpointCommand, ApplicationResult>
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
        public async Task<ApplicationResult> Handle(DeleteEndpointCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _endpointRepository.DeleteEndpoint(request.Id);

            return new ApplicationResult { Success = true };
        }
    }
}
