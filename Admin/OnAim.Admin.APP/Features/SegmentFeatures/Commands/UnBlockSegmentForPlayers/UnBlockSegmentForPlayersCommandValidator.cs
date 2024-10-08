using FluentValidation;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnBlockSegmentForPlayers;

public class UnBlockSegmentForPlayersCommandValidator : AbstractValidator<UnBlockSegmentForPlayersCommand>
{
    public UnBlockSegmentForPlayersCommandValidator()
    {
        RuleFor(x => x.SegmentId).NotEmpty();
        RuleFor(x => x.File).NotEmpty();
    }
}
