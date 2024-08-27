using FluentValidation;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Update
{
    public class UpdateEndpointGroupCommandValidator : AbstractValidator<UpdateEndpointGroupCommand>
    {
        public UpdateEndpointGroupCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.model.Name)
                .NotEmpty()
                .Matches(@"^[^\d]*$").WithMessage("Name should not contain numbers.");
        }
    }
}
