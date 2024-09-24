using FluentValidation;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnblockSegmentForPlayer;

public class UnblockSegmentForPlayerValidator : AbstractValidator<UnblockSegmentForPlayerCommand>
{
    public UnblockSegmentForPlayerValidator()
    {
        RuleFor(x => x.SegmentId).NotNull().NotEmpty();
        RuleFor(x => x.PlayerId).NotNull();
        RuleFor(x => x.ByUserId).Null();
    }
}