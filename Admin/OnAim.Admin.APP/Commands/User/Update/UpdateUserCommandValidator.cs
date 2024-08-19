using FluentValidation;

namespace OnAim.Admin.APP.Commands.User.Update
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull()
                .WithMessage("Valid Id is required.");
            RuleFor(x => x.Model)
                .NotEmpty()
                .WithMessage("Valid Model is required.");
        }
    }
}
