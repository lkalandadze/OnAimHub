using FluentValidation;

namespace OnAim.Admin.APP.Commands.User.ProfileUpdate
{
    public class UserProfileUpdateCommandValidator : AbstractValidator<UserProfileUpdateCommand>
    {
        public UserProfileUpdateCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull();
            RuleFor(x => x.profileUpdateRequest.FirstName)
                .NotEmpty()
                .Matches(@"^[^\d]*$").WithMessage("Name should not contain numbers.");
            RuleFor(x => x.profileUpdateRequest.LastName)
                .NotEmpty()
                .Matches(@"^[^\d]*$").WithMessage("Name should not contain numbers.");
        }
    }
}
