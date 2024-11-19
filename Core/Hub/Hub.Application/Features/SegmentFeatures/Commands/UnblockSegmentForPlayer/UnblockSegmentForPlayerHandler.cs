using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities.DbEnums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.SegmentFeatures.Commands.UnblockSegmentForPlayer;

public class UnblockSegmentForPlayerHandler : IRequestHandler<UnblockSegmentForPlayerCommand>
{
    private readonly ISegmentRepository _segmentRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IPlayerSegmentActService _playerSegmentActService;
    private readonly IUnitOfWork _unitOfWork;

    public UnblockSegmentForPlayerHandler(ISegmentRepository segmentRepository, IPlayerRepository playerRepository, IPlayerSegmentActService playerSegmentActService, IUnitOfWork unitOfWork)
    {
        _segmentRepository = segmentRepository;
        _playerRepository = playerRepository;
        _playerSegmentActService = playerSegmentActService;
        _unitOfWork = unitOfWork;
    }


    public async Task<Unit> Handle(UnblockSegmentForPlayerCommand request, CancellationToken cancellationToken)
    {
        var segment = await _segmentRepository.Query(s => s.Id == request.SegmentId).FirstOrDefaultAsync();

        if (segment == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Segment with the specified ID: [{request.SegmentId}] was not found.");
        }

        var player = await _playerRepository.OfIdAsync(request.PlayerId);

        if (player == null)
        {
            throw new ApiException(ApiExceptionCodeTypes.KeyNotFound, $"Player with the specified ID: [{request.PlayerId}] was not found.");
        }

        segment.UnblockPlayers([player]);

        _segmentRepository.Update(segment);
        await _playerSegmentActService.CreateActWithHistoryAsync(PlayerSegmentActType.Unblock, [request.PlayerId], request.SegmentId, request.ByUserId);

        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}