using FluentValidation;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnassignSegmentToPlayer;

public class UnassignSegmentToPlayerValidator : AbstractValidator<UnassignSegmentToPlayerCommand>
{
    public UnassignSegmentToPlayerValidator()
    {
        RuleFor(x => x.SegmentId).NotNull().NotEmpty();
        RuleFor(x => x.PlayerId).NotNull();
        RuleFor(x => x.ByUserId).Null();
    }
}