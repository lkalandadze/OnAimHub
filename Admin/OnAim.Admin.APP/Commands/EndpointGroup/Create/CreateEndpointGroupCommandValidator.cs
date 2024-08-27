using FluentValidation;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Create
{
    public class CreateEndpointGroupCommandValidator : AbstractValidator<CreateEndpointGroupCommand>
    {
        public CreateEndpointGroupCommandValidator()
        {
            RuleFor(x => x.Model.Name)
                .NotEmpty()
                .Matches(@"^[^\d]*$").WithMessage("Name should not contain numbers.");
        }
    }
}
