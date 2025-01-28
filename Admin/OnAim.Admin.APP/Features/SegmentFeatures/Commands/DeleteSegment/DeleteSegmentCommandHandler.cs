using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Segment;
using ValidationException = FluentValidation.ValidationException;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.Delete;

public class DeleteSegmentCommandHandler : ICommandHandler<DeleteSegmentCommand, ApplicationResult<bool>>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<DeleteSegmentCommand> _validator;

    public DeleteSegmentCommandHandler(ISegmentService segmentService, IValidator<DeleteSegmentCommand> validator)
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult<bool>> Handle(DeleteSegmentCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _segmentService.DeleteSegment(request.Id);
    }
}
