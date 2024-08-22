using FluentValidation;
using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Update
{
    public class UpdateEndpointGroupCommandHandler : IRequestHandler<UpdateEndpointGroupCommand, ApplicationResult>
    {
        private readonly IEndpointGroupRepository _endpointGroupRepository;
        private readonly IValidator<UpdateEndpointGroupCommand> _validator;

        public UpdateEndpointGroupCommandHandler(
            IEndpointGroupRepository endpointGroupRepository,
            IValidator<UpdateEndpointGroupCommand> validator
            )
        {
            _endpointGroupRepository = endpointGroupRepository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(UpdateEndpointGroupCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _endpointGroupRepository.UpdateAsync(request.Id, request.model);

            return new ApplicationResult
            {
                Success = true,
                Data = request.model.Name,
                Errors = null
            };
        }
    }
}
