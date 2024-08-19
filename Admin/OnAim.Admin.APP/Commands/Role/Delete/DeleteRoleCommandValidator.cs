using FluentValidation;

namespace OnAim.Admin.APP.Commands.Role.Delete
{
    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
