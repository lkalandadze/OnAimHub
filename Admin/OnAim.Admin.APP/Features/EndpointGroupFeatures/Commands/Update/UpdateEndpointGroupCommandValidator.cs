using FluentValidation;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Update;

public class UpdateEndpointGroupCommandValidator : AbstractValidator<UpdateEndpointGroupCommand>
{
    public UpdateEndpointGroupCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Model.Name)
            .NotEmpty()
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name should only contain letters and spaces, no numbers or symbols.");
    }
}
