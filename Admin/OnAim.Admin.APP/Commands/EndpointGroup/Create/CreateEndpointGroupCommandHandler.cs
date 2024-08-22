using FluentValidation;
using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Create
{
    public class CreateEndpointGroupCommandHandler : IRequestHandler<CreateEndpointGroupCommand, ApplicationResult>
    {
        private readonly IEndpointGroupRepository _endpointGroupRepository;
        private readonly IEndpointRepository _endpointRepository;
        private readonly IValidator<CreateEndpointGroupCommand> _validator;

        public CreateEndpointGroupCommandHandler(
            IEndpointGroupRepository endpointGroupRepository,
            IEndpointRepository endpointRepository,
            IValidator<CreateEndpointGroupCommand> validator
            )
        {
            _endpointGroupRepository = endpointGroupRepository;
            _endpointRepository = endpointRepository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(CreateEndpointGroupCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _endpointGroupRepository.AddAsync(request.Model);

            return new ApplicationResult
            {
                Success = true,
                Data = request.Model.Name,
                Errors = null
            };
        }
    }
}
