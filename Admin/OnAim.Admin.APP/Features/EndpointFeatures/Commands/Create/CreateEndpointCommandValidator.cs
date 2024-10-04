using FluentValidation;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Create;

public class CreateEndpointCommandValidator : AbstractValidator<CreateEndpointCommand>
{
    public CreateEndpointCommandValidator()
    {
        RuleForEach(x => x.Endpoints).ChildRules(endpoint =>
        {
            endpoint.RuleFor(x => x.Name)
                .NotEmpty()
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name should only contain letters and spaces, no numbers or symbols.");

            endpoint.RuleFor(x => x.Description)
                .NotEmpty();
        });
    }
}
