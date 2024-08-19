using FluentValidation;

namespace OnAim.Admin.APP.Commands.EndPoint.Delete
{
    public class DeleteEndpointCommandValidator : AbstractValidator<DeleteEndpointCommand>
    {
        public DeleteEndpointCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
