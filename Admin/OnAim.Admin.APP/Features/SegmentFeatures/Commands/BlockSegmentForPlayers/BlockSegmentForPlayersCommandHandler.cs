using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.HubServices.Segment;
using ValidationException = FluentValidation.ValidationException;
namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.BlockSegmentForPlayers;

public class BlockSegmentForPlayersCommandHandler : ICommandHandler<BlockSegmentForPlayersCommand, ApplicationResult<object>>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<BlockSegmentForPlayersCommand> _validator;

    public BlockSegmentForPlayersCommandHandler(ISegmentService segmentService, IValidator<BlockSegmentForPlayersCommand> validator)
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult<object>> Handle(BlockSegmentForPlayersCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await _segmentService.BlockSegmentForPlayers(request.SegmentId, request.File);
    }
}
