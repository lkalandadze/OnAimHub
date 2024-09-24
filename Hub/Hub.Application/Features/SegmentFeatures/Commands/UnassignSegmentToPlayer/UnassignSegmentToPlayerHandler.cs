using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Entities;
using MediatR;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnassignSegmentToPlayer;

public class UnassignSegmentToPlayerHandler : IRequestHandler<UnassignSegmentToPlayerCommand>
{
    private readonly IPlayerSegmentService _playerSegmentService;
    private readonly IPlayerSegmentActService _playerSegmentActService;
    private readonly IUnitOfWork _unitOfWork;

    public UnassignSegmentToPlayerHandler(IPlayerSegmentService playerSegmentService, IPlayerSegmentActService playerSegmentActService, IUnitOfWork unitOfWork)
    {
        _playerSegmentService = playerSegmentService;
        _playerSegmentActService = playerSegmentActService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UnassignSegmentToPlayerCommand request, CancellationToken cancellationToken)
    {
        await _playerSegmentActService.CreateActWithHistoryAsync(PlayerSegmentActType.Unasssign, [request.PlayerId], request.SegmentId, request.ByUserId);
        _playerSegmentService.UnassignPlayersToSegment([request.PlayerId], request.SegmentId);

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}