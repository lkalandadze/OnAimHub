using FluentValidation;

namespace OnAim.Admin.APP.Features.RoleFeatures.Commands.Update;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Model.Name)
            .NotEmpty()
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name should only contain letters and spaces, no numbers or symbols.");
    }
}
