using FluentValidation;

namespace OnAim.Admin.APP.Commands.User.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.")
                .WithMessage("Password must be at least 6 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
        }
    }
}
