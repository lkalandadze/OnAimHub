using FluentValidation;

namespace Hub.Application.Features.SegmentFeatures.Commands.CreateSegment;

public class CreateSegmentValidator : AbstractValidator<CreateSegmentCommand>
{
    public CreateSegmentValidator()
    {
        RuleFor(x => x.Description).NotNull().NotEmpty();
        //TODO ...
    }
}