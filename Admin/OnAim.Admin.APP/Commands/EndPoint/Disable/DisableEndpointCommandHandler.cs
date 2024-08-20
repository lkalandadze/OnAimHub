using FluentValidation;
using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndPoint.Disable
{
    public class DisableEndpointCommandHandler : IRequestHandler<DisableEndpointCommand, ApplicationResult>
    {
        private readonly IEndpointRepository _endpointRepository;
        private readonly IValidator<DisableEndpointCommand> _validator;

        public DisableEndpointCommandHandler(
            IEndpointRepository endpointRepository,
            IValidator<DisableEndpointCommand> validator
            )
        {
            _endpointRepository = endpointRepository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(DisableEndpointCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var result = await _endpointRepository.DisableEndpointAsync(request.EndpointId);

            return new ApplicationResult
            {
                Success = true,
                Data = result,
                Errors = null
            };
        }
    }
}
