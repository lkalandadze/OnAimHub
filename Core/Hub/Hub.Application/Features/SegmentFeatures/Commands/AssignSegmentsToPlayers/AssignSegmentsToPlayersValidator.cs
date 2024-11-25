using FluentValidation;

namespace Hub.Application.Features.SegmentFeatures.Commands.AssignSegmentsToPlayers;

public class AssignSegmentsToPlayersValidator : AbstractValidator<AssignSegmentsToPlayersCommand>
{
    public AssignSegmentsToPlayersValidator()
    {
        RuleFor(x => x.SegmentIds).NotNull().NotEmpty();

        RuleFor(x => x.File).NotNull();
    }
}