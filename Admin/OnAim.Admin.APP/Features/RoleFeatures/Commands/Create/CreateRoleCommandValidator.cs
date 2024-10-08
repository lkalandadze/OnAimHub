using FluentValidation;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Create;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Request.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("Name is required.")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name should only contain letters and spaces, no numbers or symbols.");
    }
}
