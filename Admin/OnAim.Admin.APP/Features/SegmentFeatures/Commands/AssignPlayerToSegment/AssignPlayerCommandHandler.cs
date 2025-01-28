using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Segment;
using ValidationException = FluentValidation.ValidationException;
namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.AssignPlayer;

public class AssignPlayerCommandHandler : ICommandHandler<AssignPlayerCommand, ApplicationResult<bool>>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<AssignPlayerCommand> _validator;

    public AssignPlayerCommandHandler(ISegmentService segmentService, IValidator<AssignPlayerCommand> validator)
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult<bool>> Handle(AssignPlayerCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _segmentService.AssignSegmentToPlayer(request.SegmentId, request.PlayerId);
    }
}
