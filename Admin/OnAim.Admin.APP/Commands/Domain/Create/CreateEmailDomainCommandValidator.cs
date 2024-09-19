using FluentValidation;

namespace OnAim.Admin.APP.Commands.Domain.Create
{
    public class CreateEmailDomainCommandValidator : AbstractValidator<CreateEmailDomainCommand>
    {
        public CreateEmailDomainCommandValidator()
        {
            RuleFor(x => x.Domain).NotNull().NotEmpty();
        }
    }
}
