using FluentValidation;

namespace OnAim.Admin.APP.Commands.User.AssignRole
{
    public class AssignRoleToUserCommandValidator : AbstractValidator<AssignRoleToUserCommand>
    {
        public AssignRoleToUserCommandValidator()
        {
            RuleFor(x => x.RoleId)
                .NotEmpty()
                .NotNull()
                .WithMessage("Valid RoleId is required.");
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("Valid UserId is required.");
        }
    }
}
