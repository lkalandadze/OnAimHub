using FluentValidation;
using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.Role.Create
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ApplicationResult>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IValidator<CreateRoleCommand> _validator;

        public CreateRoleCommandHandler(
            IRoleRepository roleRepository,
            IValidator<CreateRoleCommand> validator
            )
        {
            _roleRepository = roleRepository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var role = await _roleRepository.CreateRoleAsync(request.request);

            if (role == null)
            {
                throw new Exception("Role Not Created");
            }

            return new ApplicationResult
            {
                Success = true,
                Data = role,
                Errors = null
            };
        }
    }
}
