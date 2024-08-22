using FluentValidation;

namespace OnAim.Admin.APP.Commands.EndPoint.Create
{
    public class CreateEndpointCommandValidator : AbstractValidator<CreateEndpointCommand>
    {
        public CreateEndpointCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
