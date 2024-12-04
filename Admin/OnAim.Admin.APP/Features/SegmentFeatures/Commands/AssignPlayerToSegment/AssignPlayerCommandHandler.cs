using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Segment;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.AssignPlayer;

public class AssignPlayerCommandHandler : ICommandHandler<AssignPlayerCommand, ApplicationResult>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<AssignPlayerCommand> _validator;

    public AssignPlayerCommandHandler(ISegmentService segmentService, IValidator<AssignPlayerCommand> validator)
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(AssignPlayerCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _segmentService.AssignSegmentToPlayer(request.SegmentId, request.PlayerId);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}
