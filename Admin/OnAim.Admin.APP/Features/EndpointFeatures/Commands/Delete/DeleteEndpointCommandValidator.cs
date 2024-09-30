using FluentValidation;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Commands.Delete;

public class DeleteEndpointCommandValidator : AbstractValidator<DeleteEndpointCommand>
{
    public DeleteEndpointCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty();
    }
}
