using FluentValidation;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnassignSegmentsToPlayers;

public class UnassignSegmentsToPlayersValidator : AbstractValidator<UnassignSegmentsToPlayersCommand>
{
    public UnassignSegmentsToPlayersValidator()
    {
        RuleFor(x => x.SegmentIds).NotNull().NotEmpty();

        RuleFor(x => x.File).NotNull();
    }
}