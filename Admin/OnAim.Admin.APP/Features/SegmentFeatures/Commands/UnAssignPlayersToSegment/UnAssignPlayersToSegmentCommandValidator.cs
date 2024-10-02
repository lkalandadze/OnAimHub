using FluentValidation;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnAssignPlayersToSegment;

public class UnAssignPlayersToSegmentCommandValidator : AbstractValidator<UnAssignPlayersToSegmentCommand>
{
    public UnAssignPlayersToSegmentCommandValidator()
    {
        RuleFor(x => x.SegmentId).NotEmpty();   
        RuleFor(x => x.File).NotEmpty();
    }
}
