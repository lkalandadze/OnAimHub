using FluentValidation;

namespace OnAim.Admin.APP.Commands.User.Login
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Model.Email)
                .NotEmpty().EmailAddress()
                .WithMessage("Valid email is required.");
            RuleFor(x => x.Model.Password)
                .NotEmpty()
                .WithMessage("Password is required.");
        }
    }
}
