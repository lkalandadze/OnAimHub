using FluentValidation;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.Create;

public class RegisterUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
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
