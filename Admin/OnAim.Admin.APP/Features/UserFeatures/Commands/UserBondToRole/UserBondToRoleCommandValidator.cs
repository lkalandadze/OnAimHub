using FluentValidation;

namespace OnAim.Admin.APP.Feature.UserFeature.Commands.UserBondToRole;

public class UserBondToRoleCommandValidator : AbstractValidator<UserBondToRoleCommand>
{
    public UserBondToRoleCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Roles).NotEmpty();
    }
}
