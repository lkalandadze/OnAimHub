using FluentValidation;
using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.User.AssignRole
{
    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, ApplicationResult>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly IValidator<AssignRoleToUserCommand> _validator;

        public AssignRoleToUserCommandHandler(
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IMediator mediator,
            IValidator<AssignRoleToUserCommand> validator
            )
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _mediator = mediator;
            _validator = validator;
        }
        public async Task<ApplicationResult> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetById(request.UserId);

            var role = _roleRepository.GetRoleById(request.RoleId);

            var assign = _roleRepository.AssignRoleToUserAsync(user.Id, role.Result.Id);

            return new ApplicationResult
            {
                Success = true,
            };
        }
    }
}
