using FluentValidation;

namespace OnAim.Admin.APP.Features.DomainFeatures.Commands.Delete;

public class DeleteEmailDomainCommandValidator : AbstractValidator<DeleteEmailDomainCommand>
{
    public DeleteEmailDomainCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
