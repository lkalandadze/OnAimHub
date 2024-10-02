using FluentValidation;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.AssignPlayersToSegment;

public class AssignSegmentToPlayersCommandValidator : AbstractValidator<AssignSegmentToPlayersCommand>
{
    public AssignSegmentToPlayersCommandValidator()
    {
        RuleFor(x => x.SegmentId).NotEmpty();
        RuleFor(x => x.File).NotEmpty();
    }
}
