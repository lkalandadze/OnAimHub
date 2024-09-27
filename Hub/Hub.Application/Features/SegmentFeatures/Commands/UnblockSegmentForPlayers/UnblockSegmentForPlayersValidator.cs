using FluentValidation;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnblockSegmentForPlayers;

public class UnblockSegmentForPlayersValidator : AbstractValidator<UnblockSegmentForPlayersCommand>
{
    public UnblockSegmentForPlayersValidator()
    {
        RuleFor(x => x.SegmentId).NotNull().NotEmpty();
        RuleFor(x => x.File).NotNull();
        RuleFor(x => x.ByUserId).Null();
    }
}