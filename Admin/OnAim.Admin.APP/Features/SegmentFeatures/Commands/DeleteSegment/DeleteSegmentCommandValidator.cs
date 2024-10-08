using FluentValidation;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.Delete;

public class DeleteSegmentCommandValidator : AbstractValidator<DeleteSegmentCommand>
{
    public DeleteSegmentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
