using FluentValidation;

namespace Hub.Application.Features.SegmentFeatures.Commands.BlockSegmentForPlayers;

public class BlockSegmentForPlayersValidator : AbstractValidator<BlockSegmentForPlayersCommand>
{
    public BlockSegmentForPlayersValidator()
    {
        RuleFor(x => x.SegmentId).NotNull().NotEmpty();
        RuleFor(x => x.File).NotNull();
        RuleFor(x => x.ByUserId).Null();
    }
}