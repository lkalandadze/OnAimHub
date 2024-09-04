using FluentValidation;

namespace OnAim.Admin.APP.Commands.User.Domain
{
    public class CreateEmailDomainCommandValidator : AbstractValidator<CreateEmailDomainCommand>
    {
        public CreateEmailDomainCommandValidator()
        {
            RuleFor(x => x.Domain).NotNull().NotEmpty();
        }
    }
}
