using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;

namespace Hub.Application.Services.Concrete;

public class PlayerBlockedSegmentService : IPlayerBlockedSegmentService
{
    private readonly IPlayerBlockedSegmentRepository _playerBlockedSegmentRepository;

    public PlayerBlockedSegmentService(IPlayerBlockedSegmentRepository playerBlockedSegmentRepository)
    {
        _playerBlockedSegmentRepository = playerBlockedSegmentRepository;
    }

    public async Task BlockPlayerSegmentAsync(IEnumerable<int> playerIds, string segmentId)
    {
        foreach (var playerId in playerIds)
        {
            var playerBlockedSegment = new PlayerBlockedSegment(playerId, segmentId);
            await _playerBlockedSegmentRepository.InsertAsync(playerBlockedSegment);
        }
    }

    public void UnblockPlayerSegment(IEnumerable<int> playerIds, string segmentId)
    {
        foreach (var playerId in playerIds)
        {
            var playerBlockedSegment = new PlayerBlockedSegment(playerId, segmentId);
            _playerBlockedSegmentRepository.Delete(playerBlockedSegment);
        }
    }
}