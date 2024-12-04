using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Command;
using FluentValidation;
using OnAim.Admin.APP.Services.HubServices.Segment;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.AssignPlayersToSegment;

public class AssignSegmentToPlayersCommandHandler : ICommandHandler<AssignSegmentToPlayersCommand, ApplicationResult>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<AssignSegmentToPlayersCommand> _validator;

    public AssignSegmentToPlayersCommandHandler(ISegmentService segmentService, IValidator<AssignSegmentToPlayersCommand> validator)
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(AssignSegmentToPlayersCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _segmentService.AssignSegmentToPlayers(request.SegmentId, request.File);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}
