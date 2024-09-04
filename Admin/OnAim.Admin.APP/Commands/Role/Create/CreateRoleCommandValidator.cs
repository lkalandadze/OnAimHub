using FluentValidation;

namespace OnAim.Admin.APP.Commands.Role.Create
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.Request.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Name is required.")
                .Matches(@"^[^\d]*$").WithMessage("Name should not contain numbers.");
        }
    }
}
