using FluentValidation;

namespace OnAim.Admin.APP.Commands.Domain.Delete
{
    public class DeleteEmailDomainCommandValidator : AbstractValidator<DeleteEmailDomainCommand>
    {
        public DeleteEmailDomainCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
