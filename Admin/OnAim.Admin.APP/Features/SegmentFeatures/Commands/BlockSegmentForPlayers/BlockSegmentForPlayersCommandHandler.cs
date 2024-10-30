using FluentValidation;
using OnAim.Admin.APP.CQRS.Command;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.SegmentFeatures.Commands.BlockSegmentForPlayers;

public class BlockSegmentForPlayersCommandHandler : ICommandHandler<BlockSegmentForPlayersCommand, ApplicationResult>
{
    private readonly ISegmentService _segmentService;
    private readonly IValidator<BlockSegmentForPlayersCommand> _validator;

    public BlockSegmentForPlayersCommandHandler(ISegmentService segmentService, IValidator<BlockSegmentForPlayersCommand> validator)
    {
        _segmentService = segmentService;
        _validator = validator;
    }

    public async Task<ApplicationResult> Handle(BlockSegmentForPlayersCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var result = await _segmentService.BlockSegmentForPlayers(request.SegmentId, request.File);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}
