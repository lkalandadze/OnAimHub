using FluentValidation;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.Create;

public class CreateSegmentCommandValidator : AbstractValidator<CreateSegmentCommand>
{
    public CreateSegmentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
