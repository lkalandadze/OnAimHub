using FluentValidation;

namespace OnAim.Admin.APP.Commands.EndPoint.Disable
{
    public class DisableEndpointCommandValidator : AbstractValidator<DisableEndpointCommand>
    {
        public DisableEndpointCommandValidator()
        {
            RuleFor(x => x.EndpointId).NotEmpty().NotNull();
        }
    }
}
