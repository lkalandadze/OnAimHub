using FluentValidation;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.ProfileUpdate;

public class UserProfileUpdateCommandValidator : AbstractValidator<UserProfileUpdateCommand>
{
    public UserProfileUpdateCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull();
        RuleFor(x => x.ProfileUpdateRequest.FirstName)
            .NotEmpty()
            .Matches(@"^[^\d]*$").WithMessage("Name should not contain numbers.");
        RuleFor(x => x.ProfileUpdateRequest.LastName)
            .NotEmpty()
            .Matches(@"^[^\d]*$").WithMessage("Name should not contain numbers.");
    }
}
