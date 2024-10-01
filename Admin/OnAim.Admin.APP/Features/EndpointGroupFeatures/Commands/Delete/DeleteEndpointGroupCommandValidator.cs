using FluentValidation;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Delete;

public class DeleteEndpointGroupCommandValidator : AbstractValidator<DeleteEndpointGroupCommand>
{
    public DeleteEndpointGroupCommandValidator()
    {
        RuleFor(x => x.GroupIds).NotEmpty().NotNull();
    }
}
