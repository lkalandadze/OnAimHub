using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Lib.Helpers;

namespace Hub.Application.Features.SegmentFeatures.Commands.AssignSegmentToPlayers;

public class AssignSegmentToPlayersHandler : IRequestHandler<AssignSegmentToPlayersCommand>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IPlayerSegmentService _playerSegmentService;
    private readonly IPlayerSegmentActService _playerSegmentActService;
    private readonly IUnitOfWork _unitOfWork;

    public AssignSegmentToPlayersHandler(IPlayerRepository playerRepository, IPlayerSegmentService playerSegmentService, IPlayerSegmentActService playerSegmentActService, IUnitOfWork unitOfWork)
    {
        _playerRepository = playerRepository;
        _playerSegmentService = playerSegmentService;
        _playerSegmentActService = playerSegmentActService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AssignSegmentToPlayersCommand request, CancellationToken cancellationToken)
    {
        var playerIds = await request.File.ReadExcelFirstColumnAsync<int>();

        if (playerIds == null || playerIds.Count == 0)
        {
            throw new ApiException(ApiExceptionCodeTypes.ValidationFailed, "No player IDs could be retrieved from the Excel file.");
        }

        //TODO: move to PlayerService and use also in auth handler
        foreach (var playerId in playerIds)
        {
            var player = await _playerRepository.OfIdAsync(playerId);

            if (player == null)
            {
                player = new Player(playerId, string.Empty);
                
                await _playerRepository.InsertAsync(player);
            }

            await _unitOfWork.SaveAsync();
        }

        await _playerSegmentActService.CreateActWithHistoryAsync(PlayerSegmentActType.Assign, playerIds, request.SegmentId, request.ByUserId);
        await _playerSegmentService.AssignPlayersToSegmentAsync(playerIds, request.SegmentId);

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}