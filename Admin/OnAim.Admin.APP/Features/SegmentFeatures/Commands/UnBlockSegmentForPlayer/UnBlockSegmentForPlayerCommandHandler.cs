using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.UnBlockPlayer;

public class UnBlockSegmentForPlayerCommandHandler : ICommandHandler<UnBlockSegmentForPlayerCommand, ApplicationResult>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<UnBlockSegmentForPlayerCommand> _validator;

    public UnBlockSegmentForPlayerCommandHandler(ISegmentService segmentService, IValidator<UnBlockSegmentForPlayerCommand> validator
 )
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(UnBlockSegmentForPlayerCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _segmentService.UnBlockSegmentForPlayer(request.SegmentId, request.PlayerId);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}
