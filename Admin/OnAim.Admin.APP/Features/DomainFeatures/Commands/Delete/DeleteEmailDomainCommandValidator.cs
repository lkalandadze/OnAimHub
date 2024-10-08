using FluentValidation;

namespace OnAim.Admin.APP.Features.DomainFeatures.Commands.Delete;

public class DeleteEmailDomainCommandValidator : AbstractValidator<DeleteEmailDomainCommand>
{
    public DeleteEmailDomainCommandValidator()
    {
        RuleForEach(x => x.Ids).NotEmpty();
    }
}
