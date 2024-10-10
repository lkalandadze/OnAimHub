using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnAssignPlayer;

public class UnAssignPlayerCommandHandler : ICommandHandler<UnAssignPlayerCommand, ApplicationResult>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<UnAssignPlayerCommand> _validator;

    public UnAssignPlayerCommandHandler(ISegmentService segmentService, IValidator<UnAssignPlayerCommand> validator)
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(UnAssignPlayerCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _segmentService.UnAssignSegmentForPlayer(request.SegmentId, request.PlayerId);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}
