using FluentValidation;
using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.RemoveRole
{
    public class RemoveRoleCommandHandler : IRequestHandler<RemoveRoleCommand, ApplicationResult>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IValidator<RemoveRoleCommand> _validator;

        public RemoveRoleCommandHandler(
            IRoleRepository roleRepository,
            IValidator<RemoveRoleCommand> validator
            )
        {
            _roleRepository = roleRepository;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _roleRepository.RemoveRoleFromUserAsync(request.UserId, request.RoleId);

            return new ApplicationResult
            {
                Success = true,
                Data = null,
                Errors = null
            };
        }
    }
}
