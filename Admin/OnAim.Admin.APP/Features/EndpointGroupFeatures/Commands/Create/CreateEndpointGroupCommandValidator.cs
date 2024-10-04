using FluentValidation;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Create;

public class CreateEndpointGroupCommandValidator : AbstractValidator<CreateEndpointGroupCommand>
{
    public CreateEndpointGroupCommandValidator()
    {
        RuleFor(x => x.Model.Name)
                .NotEmpty()
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name should only contain letters and spaces, no numbers or symbols.");
    }
}
