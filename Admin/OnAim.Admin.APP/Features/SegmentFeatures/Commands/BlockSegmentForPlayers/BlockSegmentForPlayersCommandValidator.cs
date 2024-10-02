using FluentValidation; 

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.BlockSegmentForPlayers
{
    public class BlockSegmentForPlayersCommandValidator : AbstractValidator<BlockSegmentForPlayersCommand>
    {
        public BlockSegmentForPlayersCommandValidator()
        {
            RuleFor(x => x.SegmentId).NotEmpty();
            RuleFor(x => x.File).NotEmpty();
        }
    }
}
