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
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .NotNull()
                .Matches(@"^[^\d]*$").WithMessage("Name should not contain numbers.");
            RuleFor(x => x.LastName)
                .NotEmpty()
                .NotNull()
                .Matches(@"^[^\d]*$").WithMessage("Name should not contain numbers.");
        }
    }
}
