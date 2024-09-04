using FluentValidation;

namespace OnAim.Admin.APP.Commands.EndpointGroup.GroupBondToEndpoint
{
    public class GroupBondToEndpointCommandValidator : AbstractValidator<GroupBondToEndpointCommand>
    {
        public GroupBondToEndpointCommandValidator()
        {
            RuleFor(x => x.GroupId).NotEmpty().NotNull();
            RuleFor(x => x.Endpoints).NotEmpty().NotNull();
        }
    }
}
