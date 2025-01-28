using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Segment;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnAssignPlayer;

public class UnAssignPlayerCommandHandler : ICommandHandler<UnAssignPlayerCommand, ApplicationResult<bool>>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<UnAssignPlayerCommand> _validator;

    public UnAssignPlayerCommandHandler(ISegmentService segmentService, IValidator<UnAssignPlayerCommand> validator)
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult<bool>> Handle(UnAssignPlayerCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _segmentService.UnAssignSegmentForPlayer(request.SegmentId, request.PlayerId);
    }
}
