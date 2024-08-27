using FluentValidation;

namespace OnAim.Admin.APP.Commands.EndPoint.Create
{
    public class CreateEndpointCommandValidator : AbstractValidator<CreateEndpointCommand>
    {
        public CreateEndpointCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches(@"^[^\d]*$").WithMessage("Name should not contain numbers.");
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
