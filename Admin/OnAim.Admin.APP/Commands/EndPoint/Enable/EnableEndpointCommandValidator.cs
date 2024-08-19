using FluentValidation;

namespace OnAim.Admin.APP.Commands.EndPoint.Enable
{
    public class EnableEndpointCommandValidator : AbstractValidator<EnableEndpointCommand>
    {
        public EnableEndpointCommandValidator()
        {
            RuleFor(x => x.EndpointId).NotEmpty().NotNull();
        }
    }
}
