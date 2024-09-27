using FluentValidation;

namespace Hub.Application.Features.SegmentFeatures.Commands.BlockSegmentForPlayer;

public class BlockSegmentForPlayerValidator : AbstractValidator<BlockSegmentForPlayerCommand>
{
    public BlockSegmentForPlayerValidator()
    {
        RuleFor(x => x.SegmentId).NotNull().NotEmpty();
        RuleFor(x => x.PlayerId).NotNull();
        RuleFor(x => x.ByUserId).Null();
    }
}