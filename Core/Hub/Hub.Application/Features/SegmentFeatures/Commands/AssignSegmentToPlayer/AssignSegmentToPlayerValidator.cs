using FluentValidation;

namespace Hub.Application.Features.SegmentFeatures.Commands.AssignSegmentToPlayer;

public class AssignSegmentToPlayerValidator : AbstractValidator<AssignSegmentToPlayerCommand>
{
    public AssignSegmentToPlayerValidator()
    {
        RuleFor(x => x.SegmentId).NotNull().NotEmpty();

        RuleFor(x => x.PlayerId).NotNull();
    }
}