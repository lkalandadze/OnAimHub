using FluentValidation;
using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.Role.Delete
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, ApplicationResult>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IValidator<DeleteRoleCommand> _validator;

        public DeleteRoleCommandHandler(
            IRoleRepository roleRepository,
            IValidator<DeleteRoleCommand> validator
            )
        {
            _roleRepository = roleRepository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _roleRepository.DeleteRole(request.Id);

            return new ApplicationResult
            {
                Success = true,
                Errors = null,
                Data = null
            };
        }
    }
}
