using FluentValidation;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ForgotPassword;

public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();

    }
}
