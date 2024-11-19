using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions;
using Hub.Domain.Entities.DbEnums;
using MediatR;

namespace Hub.Application.Features.SegmentFeatures.Commands.AssignSegmentToPlayer;

public class AssignSegmentToPlayerHandler : IRequestHandler<AssignSegmentToPlayerCommand>
{
    private readonly IPlayerSegmentService _playerSegmentService;
    private readonly IPlayerSegmentActService _playerSegmentActService;
    private readonly IUnitOfWork _unitOfWork;

    public AssignSegmentToPlayerHandler(IPlayerSegmentService playerSegmentService, IPlayerSegmentActService playerSegmentActService, IUnitOfWork unitOfWork)
    {
        _playerSegmentService = playerSegmentService;
        _playerSegmentActService = playerSegmentActService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AssignSegmentToPlayerCommand request, CancellationToken cancellationToken)
    {
        await _playerSegmentService.AssignPlayersToSegmentAsync([request.PlayerId], request.SegmentId);
        await _playerSegmentActService.CreateActWithHistoryAsync(PlayerSegmentActType.Assign, [request.PlayerId], request.SegmentId, request.ByUserId);

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}