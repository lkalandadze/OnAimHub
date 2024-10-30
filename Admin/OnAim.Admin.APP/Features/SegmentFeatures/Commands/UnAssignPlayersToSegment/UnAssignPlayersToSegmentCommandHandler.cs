using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnAssignPlayersToSegment;

public class UnAssignPlayersToSegmentCommandHandler : ICommandHandler<UnAssignPlayersToSegmentCommand, ApplicationResult>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<UnAssignPlayersToSegmentCommand> _validator;

    public UnAssignPlayersToSegmentCommandHandler(ISegmentService segmentService, IValidator<UnAssignPlayersToSegmentCommand> validator)
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(UnAssignPlayersToSegmentCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _segmentService.UnAssignPlayersToSegment(request.SegmentId, request.File);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}
