using FluentValidation;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.AssignPlayer
{
    public class AssignPlayerCommandValidator : AbstractValidator<AssignPlayerCommand>
    {
        public AssignPlayerCommandValidator()
        {
            RuleFor(x => x.PlayerId).NotEmpty();
            RuleFor(x => x.SegmentId).NotEmpty();
        }
    }
}
