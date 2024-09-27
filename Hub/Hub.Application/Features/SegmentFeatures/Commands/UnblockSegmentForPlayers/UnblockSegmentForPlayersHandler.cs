using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Entities.DbEnums;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Lib.Helpers;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnblockSegmentForPlayers;

public class UnblockSegmentForPlayersHandler : IRequestHandler<UnblockSegmentForPlayersCommand>
{
    private readonly IPlayerService _playerService;
    private readonly IPlayerBlockedSegmentService _playerBlockedSegmentService;
    private readonly IPlayerSegmentActService _playerSegmentActService;
    private readonly IUnitOfWork _unitOfWork;

    public UnblockSegmentForPlayersHandler(IPlayerService playerService, IPlayerBlockedSegmentService playerBlockedSegmentService, IPlayerSegmentActService playerSegmentActService, IUnitOfWork unitOfWork)
    {
        _playerService = playerService;
        _playerBlockedSegmentService = playerBlockedSegmentService;
        _playerSegmentActService = playerSegmentActService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UnblockSegmentForPlayersCommand request, CancellationToken cancellationToken)
    {
        var playerIds = await request.File.ReadExcelFirstColumnAsync<int>();

        if (playerIds == null || playerIds.Count == 0)
        {
            throw new ApiException(ApiExceptionCodeTypes.ValidationFailed, "No player IDs could be retrieved from the Excel file.");
        }

        await _playerService.CreatePlayersIfNotExist(playerIds);
        await _playerSegmentActService.CreateActWithHistoryAsync(PlayerSegmentActType.Unblock, playerIds, request.SegmentId, request.ByUserId, true);
        _playerBlockedSegmentService.UnblockPlayerSegment(playerIds, request.SegmentId);

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}