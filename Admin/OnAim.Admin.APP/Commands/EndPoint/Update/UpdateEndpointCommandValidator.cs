using FluentValidation;

namespace OnAim.Admin.APP.Commands.EndPoint.Update
{
    public class UpdateEndpointCommandValidator : AbstractValidator<UpdateEndpointCommand>
    {
        public UpdateEndpointCommandValidator()
        {
            //RuleFor(x => x.Endpoint.Name)
            //    .NotEmpty()
            //    .Matches(@"^[^\d]*$").WithMessage("Name should not contain numbers.");
        }
    }
}
