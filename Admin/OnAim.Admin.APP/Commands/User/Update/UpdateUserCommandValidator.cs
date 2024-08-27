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
            RuleFor(x => x.Model.FirstName)
                .NotEmpty()
                .WithMessage("Valid Model is required.")
                .Matches(@"^[^\d]*$").WithMessage("Name should not contain numbers.");
            RuleFor(x => x.Model.LastName)
                .NotEmpty()
                .WithMessage("Valid Model is required.")
                .Matches(@"^[^\d]*$").WithMessage("Name should not contain numbers.");
        }
    }
}
