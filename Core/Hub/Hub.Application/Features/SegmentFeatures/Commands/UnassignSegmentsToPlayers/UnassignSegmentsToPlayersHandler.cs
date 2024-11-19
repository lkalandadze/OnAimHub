using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions;
using Hub.Domain.Entities.DbEnums;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Lib.Helpers;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnassignSegmentsToPlayers;

public class UnassignSegmentsToPlayersHandler : IRequestHandler<UnassignSegmentsToPlayersCommand>
{
    private readonly IPlayerService _playerService;
    private readonly IPlayerSegmentService _playerSegmentService;
    private readonly IPlayerSegmentActService _playerSegmentActService;
    private readonly IUnitOfWork _unitOfWork;

    public UnassignSegmentsToPlayersHandler(IPlayerService playerService, IPlayerSegmentService playerSegmentService, IPlayerSegmentActService playerSegmentActService, IUnitOfWork unitOfWork)
    {
        _playerService = playerService;
        _playerSegmentService = playerSegmentService;
        _playerSegmentActService = playerSegmentActService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UnassignSegmentsToPlayersCommand request, CancellationToken cancellationToken)
    {
        var playerIds = await request.File.ReadExcelFirstColumnAsync<int>();

        if (playerIds == null || playerIds.Count == 0)
        {
            throw new ApiException(ApiExceptionCodeTypes.ValidationFailed, "No player IDs could be retrieved from the Excel file.");
        }

        await _playerService.CreatePlayersIfNotExist(playerIds);

        foreach (var segmentId in request.SegmentIds)
        {
            _playerSegmentService.UnassignPlayersToSegment(playerIds, segmentId);
            await _playerSegmentActService.CreateActWithHistoryAsync(PlayerSegmentActType.Unassign, playerIds, segmentId, request.ByUserId, true);
        }

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}