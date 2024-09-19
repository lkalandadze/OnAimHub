using FluentValidation;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Delete
{
    public class DeleteEndpointGroupCommandValidator : AbstractValidator<DeleteEndpointGroupCommand>
    {
        public DeleteEndpointGroupCommandValidator()
        {
            RuleFor(x => x.GroupId).NotEmpty().NotNull();
        }
    }
}
