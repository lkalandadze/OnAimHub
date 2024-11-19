using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities.DbEnums;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Lib.Helpers;

namespace Hub.Application.Features.SegmentFeatures.Commands.AssignSegmentsToPlayers;

public class AssignSegmentsToPlayersHandler : IRequestHandler<AssignSegmentsToPlayersCommand>
{
    private readonly ISegmentRepository _segmentRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IPlayerService _playerService;
    private readonly IPlayerSegmentActService _playerSegmentActService;
    private readonly IUnitOfWork _unitOfWork;

    public AssignSegmentsToPlayersHandler(ISegmentRepository segmentRepository, IPlayerRepository playerRepository, IPlayerService playerService, IPlayerSegmentActService playerSegmentActService, IUnitOfWork unitOfWork)
    {
        _segmentRepository = segmentRepository;
        _playerRepository = playerRepository;
        _playerService = playerService;
        _playerSegmentActService = playerSegmentActService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AssignSegmentsToPlayersCommand request, CancellationToken cancellationToken)
    {
        var playerIds = await request.File.ReadExcelFirstColumnAsync<int>();

        if (playerIds == null || playerIds.Count == 0)
        {
            throw new ApiException(ApiExceptionCodeTypes.ValidationFailed, "No player IDs could be retrieved from the Excel file.");
        }

        var segments = (await _segmentRepository.QueryAsync(s => request.SegmentIds.Any(sId => sId == s.Id)));

        if (segments == null || !segments.Any())
        {
            throw new ApiException(
                ApiExceptionCodeTypes.KeyNotFound,
                "No segments were found for the provided list of IDs. Please ensure the IDs are valid and correspond to existing segments."
            );
        }

        await _playerService.CreatePlayersIfNotExist(playerIds);

        var players = (await _playerRepository.QueryAsync(p => playerIds.Any(pId => pId == p.Id)));

        if (players == null || !players.Any())
        {
            throw new ApiException(
                ApiExceptionCodeTypes.KeyNotFound,
                "No players were found for the provided list of IDs. Please ensure the IDs are valid and correspond to existing players."
            );
        }

        foreach (var segment in segments)
        {
            segment.AddPlayers(players);
            _segmentRepository.Update(segment);
            await _playerSegmentActService.CreateActWithHistoryAsync(PlayerSegmentActType.Assign, playerIds, segment.Id, request.ByUserId, true);
        }

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}