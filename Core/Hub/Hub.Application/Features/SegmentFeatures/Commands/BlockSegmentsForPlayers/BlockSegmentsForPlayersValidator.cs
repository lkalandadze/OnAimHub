using FluentValidation;

namespace Hub.Application.Features.SegmentFeatures.Commands.BlockSegmentsForPlayers;

public class BlockSegmentsForPlayersValidator : AbstractValidator<BlockSegmentsForPlayersCommand>
{
    public BlockSegmentsForPlayersValidator()
    {
        RuleFor(x => x.SegmentIds).NotNull().NotEmpty();

        RuleFor(x => x.File).NotNull();
    }
}