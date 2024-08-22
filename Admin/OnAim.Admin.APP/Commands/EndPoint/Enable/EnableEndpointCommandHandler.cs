using FluentValidation;
using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndPoint.Enable
{
    public class EnableEndpointCommandHandler : IRequestHandler<EnableEndpointCommand, ApplicationResult>
    {
        private readonly IEndpointRepository _endpointRepository;
        private readonly IValidator<EnableEndpointCommand> _validator;

        public EnableEndpointCommandHandler(
            IEndpointRepository endpointRepository,
            IValidator<EnableEndpointCommand> validator
            )
        {
            _endpointRepository = endpointRepository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(EnableEndpointCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var res = await _endpointRepository.EnableEndpointAsync(request.EndpointId);

            return new ApplicationResult
            {
                Success = true,
                Data = res,
                Errors = null
            };
        }
    }
}
