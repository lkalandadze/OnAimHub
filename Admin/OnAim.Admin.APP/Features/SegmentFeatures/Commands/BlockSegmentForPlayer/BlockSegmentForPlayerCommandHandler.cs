using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Segment;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.BlockPlayer;

public class BlockSegmentForPlayerCommandHandler : ICommandHandler<BlockSegmentForPlayerCommand, ApplicationResult<bool>>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<BlockSegmentForPlayerCommand> _validator;

    public BlockSegmentForPlayerCommandHandler(ISegmentService segmentService, IValidator<BlockSegmentForPlayerCommand> validator)
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult<bool>> Handle(BlockSegmentForPlayerCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _segmentService.BlockSegmentForPlayer(request.SegmentId, request.PlayerId);
    }
}
