using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Segment;
using ValidationException = FluentValidation.ValidationException;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnAssignPlayersToSegment;

public class UnAssignPlayersToSegmentCommandHandler : ICommandHandler<UnAssignPlayersToSegmentCommand, ApplicationResult<object>>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<UnAssignPlayersToSegmentCommand> _validator;

    public UnAssignPlayersToSegmentCommandHandler(ISegmentService segmentService, IValidator<UnAssignPlayersToSegmentCommand> validator)
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult<object>> Handle(UnAssignPlayersToSegmentCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _segmentService.UnAssignPlayersToSegment(request.SegmentId, request.File);
    }
}
