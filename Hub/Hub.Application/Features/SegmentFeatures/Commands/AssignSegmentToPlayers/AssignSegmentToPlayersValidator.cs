using FluentValidation;

namespace Hub.Application.Features.SegmentFeatures.Commands.AssignSegmentToPlayers;

public class AssignSegmentToPlayersValidator : AbstractValidator<AssignSegmentToPlayersCommand>
{
    public AssignSegmentToPlayersValidator()
    {
        RuleFor(x => x.SegmentId).NotNull().NotEmpty();
        RuleFor(x => x.File).NotNull();
        RuleFor(x => x.ByUserId).Null();
    }
}