using FluentValidation;
using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndPoint.Create
{
    public class CreateEndpointCommandHandler : IRequestHandler<CreateEndpointCommand, ApplicationResult>
    {
        private readonly IEndpointRepository _endpointRepository;
        private readonly IValidator<CreateEndpointCommand> _validator;

        public CreateEndpointCommandHandler(
            IEndpointRepository endpointRepository,
            IValidator<CreateEndpointCommand> validator
            )
        {
            _endpointRepository = endpointRepository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(CreateEndpointCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var result = await _endpointRepository.CreateEndpointAsync(request.Name, request.Description, request.Type);

            if (result == null) { }

            return new ApplicationResult
            {
                Success = true,
                Data = result,
                Errors = null
            };

        }
    }
}
