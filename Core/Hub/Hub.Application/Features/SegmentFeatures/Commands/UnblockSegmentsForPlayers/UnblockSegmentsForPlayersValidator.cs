using FluentValidation;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnblockSegmentsForPlayers;

public class UnblockSegmentsForPlayersValidator : AbstractValidator<UnblockSegmentsForPlayersCommand>
{
    public UnblockSegmentsForPlayersValidator()
    {
        RuleFor(x => x.SegmentIds).NotNull().NotEmpty();

        RuleFor(x => x.File).NotNull();
    }
}