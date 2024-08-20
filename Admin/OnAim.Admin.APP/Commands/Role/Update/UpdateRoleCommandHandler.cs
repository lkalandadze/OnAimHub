using FluentValidation;
using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.Role.Update
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ApplicationResult>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IEndpointGroupRepository _endpointGroupRepository;
        private readonly IEndpointRepository _endpointRepository;
        private readonly IValidator<UpdateRoleCommand> _validator;

        public UpdateRoleCommandHandler(
            IRoleRepository roleRepository,
            IEndpointGroupRepository endpointGroupRepository,
            IEndpointRepository endpointRepository,
            IValidator<UpdateRoleCommand> validator
            )
        {
            _roleRepository = roleRepository;
            _endpointGroupRepository = endpointGroupRepository;
            _endpointRepository = endpointRepository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var role = await _roleRepository.UpdateRoleAsync(request.Id, request.Model);

            return new ApplicationResult
            {
                Success = true,
                Data = role,
                Errors = null
            };
        }
    }
}
