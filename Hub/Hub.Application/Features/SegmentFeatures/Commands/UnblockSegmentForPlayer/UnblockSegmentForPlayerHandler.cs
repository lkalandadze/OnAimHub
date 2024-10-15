using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Entities.DbEnums;
using MediatR;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnblockSegmentForPlayer;

public class UnblockSegmentForPlayerHandler : IRequestHandler<UnblockSegmentForPlayerCommand>
{
    private readonly IPlayerBlockedSegmentService _playerBlockedSegmentService;
    private readonly IPlayerSegmentActService _playerSegmentActService;
    private readonly IUnitOfWork _unitOfWork;

    public UnblockSegmentForPlayerHandler(IPlayerBlockedSegmentService playerBlockedSegmentService, IPlayerSegmentActService playerSegmentActService, IUnitOfWork unitOfWork)
    {
        _playerBlockedSegmentService = playerBlockedSegmentService;
        _playerSegmentActService = playerSegmentActService;
        _unitOfWork = unitOfWork;
    }


    public async Task<Unit> Handle(UnblockSegmentForPlayerCommand request, CancellationToken cancellationToken)
    {
        _playerBlockedSegmentService.UnblockPlayerSegment([request.PlayerId], request.SegmentId);
        await _playerSegmentActService.CreateActWithHistoryAsync(PlayerSegmentActType.Unblock, [request.PlayerId], request.SegmentId, request.ByUserId);

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}