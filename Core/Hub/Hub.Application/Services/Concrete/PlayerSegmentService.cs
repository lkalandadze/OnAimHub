using Hub.Application.Services.Abstract;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;

namespace Hub.Application.Services.Concrete;

public class PlayerSegmentService : IPlayerSegmentService
{
    private readonly IPlayerSegmentRepository _playerSegmentRepository;

    public PlayerSegmentService(IPlayerSegmentRepository playerSegmentRepository)
    {
        _playerSegmentRepository = playerSegmentRepository;
    }

    public async Task AssignPlayersToSegmentAsync(IEnumerable<int> playerIds, string segmentId)
    {
        foreach (var playerId in playerIds)
        {
            var playerSegment = new PlayerSegment(playerId, segmentId.ToLower());
            await _playerSegmentRepository.InsertAsync(playerSegment);
        }
    }

    public void UnassignPlayersToSegment(IEnumerable<int> playerIds, string segmentId)
    {
        foreach (var playerId in playerIds)
        {
            var playerSegment = new PlayerSegment(playerId, segmentId.ToLower());
            _playerSegmentRepository.Delete(playerSegment);
        }
    }
}