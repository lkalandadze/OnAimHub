using FluentValidation;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnAssignPlayer
{
    public class UnAssignPlayerCommandValidator : AbstractValidator<UnAssignPlayerCommand>
    {
        public UnAssignPlayerCommandValidator()
        {
            RuleFor(x => x.PlayerId).NotEmpty();
            RuleFor(x => x.SegmentId).NotEmpty();
        }
    }
}
