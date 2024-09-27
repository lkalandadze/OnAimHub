using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Entities.DbEnums;
using MediatR;

namespace Hub.Application.Features.SegmentFeatures.Commands.BlockSegmentForPlayer;

public class BlockSegmentForPlayerHandler : IRequestHandler<BlockSegmentForPlayerCommand>
{
    private readonly IPlayerBlockedSegmentService _playerBlockedSegmentService;
    private readonly IPlayerSegmentActService _playerSegmentActService;
    private readonly IUnitOfWork _unitOfWork;

    public BlockSegmentForPlayerHandler(IPlayerBlockedSegmentService playerBlockedSegmentService, IPlayerSegmentActService playerSegmentActService, IUnitOfWork unitOfWork)
    {
        _playerBlockedSegmentService = playerBlockedSegmentService;
        _playerSegmentActService = playerSegmentActService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(BlockSegmentForPlayerCommand request, CancellationToken cancellationToken)
    {
        await _playerSegmentActService.CreateActWithHistoryAsync(PlayerSegmentActType.Block, [request.PlayerId], request.SegmentId, request.ByUserId);
        await _playerBlockedSegmentService.BlockPlayerSegmentAsync([request.PlayerId], request.SegmentId);

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}