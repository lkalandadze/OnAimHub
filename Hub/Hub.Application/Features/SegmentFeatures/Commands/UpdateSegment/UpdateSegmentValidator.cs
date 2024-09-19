using FluentValidation;

namespace Hub.Application.Features.SegmentFeatures.Commands.UpdateSegment;

public class UpdateSegmentValidator : AbstractValidator<UpdateSegmentCommand>
{
    public UpdateSegmentValidator()
    {
        RuleFor(x => x.Description).NotNull().NotEmpty();
        //TODO ...
    }
}