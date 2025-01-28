using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Segment;
using ValidationException = FluentValidation.ValidationException;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnBlockSegmentForPlayers;

public class UnBlockSegmentForPlayersCommandHandler : ICommandHandler<UnBlockSegmentForPlayersCommand, ApplicationResult<object>>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<UnBlockSegmentForPlayersCommand> _validator;

    public UnBlockSegmentForPlayersCommandHandler(ISegmentService segmentService, IValidator<UnBlockSegmentForPlayersCommand> validator) 
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult<object>> Handle(UnBlockSegmentForPlayersCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _segmentService.UnAssignPlayersToSegment(request.SegmentId, request.File);
    }
}
