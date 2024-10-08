using FluentValidation;

namespace OnAim.Admin.APP.Features.UserFeatures.Commands.TwoFA;

public class VerifyOtpCommandValidator : AbstractValidator<VerifyOtpCommand>
{
    public VerifyOtpCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.OtpCode).NotEmpty();
    }
}
