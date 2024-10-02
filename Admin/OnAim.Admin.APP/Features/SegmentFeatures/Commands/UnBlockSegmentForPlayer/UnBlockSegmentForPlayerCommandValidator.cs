using FluentValidation;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnBlockPlayer
{
    public class UnBlockSegmentForPlayerCommandValidator : AbstractValidator<UnBlockSegmentForPlayerCommand>
    {
        public UnBlockSegmentForPlayerCommandValidator()
        {
            RuleFor(x => x.PlayerId).NotEmpty();
            RuleFor(x => x.SegmentId).NotEmpty();
        }
    }
}
