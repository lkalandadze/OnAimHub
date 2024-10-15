﻿using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions;
using Hub.Domain.Entities.DbEnums;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Lib.Helpers;

namespace Hub.Application.Features.SegmentFeatures.Commands.BlockSegmentsForPlayers;

public class BlockSegmentsForPlayersHandler : IRequestHandler<BlockSegmentsForPlayersCommand>
{
    private readonly IPlayerService _playerService;
    private readonly IPlayerBlockedSegmentService _playerBlockedSegmentService;
    private readonly IPlayerSegmentActService _playerSegmentActService;
    private readonly IUnitOfWork _unitOfWork;

    public BlockSegmentsForPlayersHandler(IPlayerService playerService, IPlayerBlockedSegmentService playerBlockedSegmentService, IPlayerSegmentActService playerSegmentActService, IUnitOfWork unitOfWork)
    {
        _playerService = playerService;
        _playerBlockedSegmentService = playerBlockedSegmentService;
        _playerSegmentActService = playerSegmentActService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(BlockSegmentsForPlayersCommand request, CancellationToken cancellationToken)
    {
        var playerIds = await request.File.ReadExcelFirstColumnAsync<int>();

        if (playerIds == null || playerIds.Count == 0)
        {
            throw new ApiException(ApiExceptionCodeTypes.ValidationFailed, "No player IDs could be retrieved from the Excel file.");
        }

        await _playerService.CreatePlayersIfNotExist(playerIds);

        foreach (var segmentId in request.SegmentIds)
        {
            await _playerBlockedSegmentService.BlockPlayerSegmentAsync(playerIds, segmentId);
            await _playerSegmentActService.CreateActWithHistoryAsync(PlayerSegmentActType.Block, playerIds, segmentId, request.ByUserId, true);
        }

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}