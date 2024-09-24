using FluentValidation;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnassignSegmentToPlayers;

public class UnassignSegmentToPlayersValidator : AbstractValidator<UnassignSegmentToPlayersCommand>
{
    public UnassignSegmentToPlayersValidator()
    {
        RuleFor(x => x.SegmentId).NotNull().NotEmpty();
        RuleFor(x => x.File).NotNull();
        RuleFor(x => x.ByUserId).Null();
    }
}