using FluentValidation;

namespace OnAim.Admin.APP.Commands.Role.RoleBondToGroup
{
    public class RoleBondToGroupCommandValidator : AbstractValidator<RoleBondToGroupCommand>
    {
        public RoleBondToGroupCommandValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty().NotNull();
            RuleFor(x => x.Groups).NotEmpty().NotNull();
        }
    }
}
