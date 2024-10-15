using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.BlockPlayer;

public class BlockSegmentForPlayerCommandHandler : ICommandHandler<BlockSegmentForPlayerCommand, ApplicationResult>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<BlockSegmentForPlayerCommand> _validator;

    public BlockSegmentForPlayerCommandHandler(ISegmentService segmentService, IValidator<BlockSegmentForPlayerCommand> validator)
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(BlockSegmentForPlayerCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _segmentService.BlockSegmentForPlayer(request.SegmentId, request.PlayerId);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}
