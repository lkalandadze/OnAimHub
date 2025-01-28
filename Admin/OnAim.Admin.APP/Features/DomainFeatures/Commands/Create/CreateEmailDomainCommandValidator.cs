namespace OnAim.Admin.APP.Features.DomainFeatures.Commands.Create;

public class CreateEmailDomainCommandValidator : AbstractValidator<CreateEmailDomainCommand>
{
    public CreateEmailDomainCommandValidator()
    {
        RuleFor(x => x.Domain).NotNull().NotEmpty();
    }
}
