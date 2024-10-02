using FluentValidation;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.BlockPlayer;

public class BlockSegmentForPlayerCommandValidator : AbstractValidator<BlockSegmentForPlayerCommand>
{
    public BlockSegmentForPlayerCommandValidator()
    {
        RuleFor(x => x.SegmentId).NotEmpty();
        RuleFor(x => x.PlayerId).NotEmpty();
    }
}
