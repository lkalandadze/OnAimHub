using FluentValidation;

namespace OnAim.Admin.APP.Commands.User.Create
{
    public class RegisterUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Valid email is required.");
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.");
        }

    }
}
