using FluentValidation;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.Delete;

public class DeleteEndpointGroupCommandValidator : AbstractValidator<DeleteEndpointGroupCommand>
{
    public DeleteEndpointGroupCommandValidator()
    {
        RuleFor(x => x.GroupId).NotEmpty().NotNull();
    }
}
