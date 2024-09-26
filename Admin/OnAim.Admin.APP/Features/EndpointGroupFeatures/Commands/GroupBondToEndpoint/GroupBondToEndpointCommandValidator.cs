using FluentValidation;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Commands.GroupBondToEndpoint;

public class GroupBondToEndpointCommandValidator : AbstractValidator<GroupBondToEndpointCommand>
{
    public GroupBondToEndpointCommandValidator()
    {
        RuleFor(x => x.GroupId).NotEmpty().NotNull();
        RuleFor(x => x.Endpoints).NotEmpty().NotNull();
    }
}
