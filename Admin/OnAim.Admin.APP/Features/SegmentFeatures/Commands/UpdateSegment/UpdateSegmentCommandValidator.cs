using FluentValidation;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.Update;

public class UpdateSegmentCommandValidator : AbstractValidator<UpdateSegmentCommand>
{
    public UpdateSegmentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
