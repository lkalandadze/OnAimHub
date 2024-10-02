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
                .Matches(@"^[^\d]*$").WithMessage("Name should not contain numbers.");

            endpoint.RuleFor(x => x.Description)
                .NotEmpty();
        });
    }
}
