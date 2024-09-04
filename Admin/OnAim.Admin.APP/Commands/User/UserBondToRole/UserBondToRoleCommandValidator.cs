using FluentValidation;

namespace OnAim.Admin.APP.Commands.User.UserBondToRole
{
    public class UserBondToRoleCommandValidator : AbstractValidator<UserBondToRoleCommand>
    {
        public UserBondToRoleCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Roles).NotEmpty();
        }
    }
}
