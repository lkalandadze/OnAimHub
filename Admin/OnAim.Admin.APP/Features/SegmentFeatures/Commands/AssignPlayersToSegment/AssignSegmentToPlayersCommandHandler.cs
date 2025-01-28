using OnAim.Admin.APP.CQRS.Command;
using ValidationException = FluentValidation.ValidationException;
using OnAim.Admin.APP.Services.HubServices.Segment;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.AssignPlayersToSegment;

public class AssignSegmentToPlayersCommandHandler : ICommandHandler<AssignSegmentToPlayersCommand, ApplicationResult<object>>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<AssignSegmentToPlayersCommand> _validator;

    public AssignSegmentToPlayersCommandHandler(ISegmentService segmentService, IValidator<AssignSegmentToPlayersCommand> validator)
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult<object>> Handle(AssignSegmentToPlayersCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _segmentService.AssignSegmentToPlayers(request.SegmentId, request.File);
    }
}
