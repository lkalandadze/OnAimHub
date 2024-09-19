using FluentValidation;

namespace OnAim.Admin.APP.Commands.User.ForgotPassword
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
           
        }
    }
}
