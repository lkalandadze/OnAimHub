using Hub.Application.Services.Abstract;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Entities.DbEnums;

namespace Hub.Application.Services.Concretel;

public class PlayerSegmentActService : IPlayerSegmentActService
{
    private readonly IPlayerSegmentActRepository _playerSegmentActRepository;
    private readonly IPlayerSegmentActHistoryRepository _playerSegmentActHistoryRepository;

    public PlayerSegmentActService(IPlayerSegmentActRepository playerSegmentActRepository, IPlayerSegmentActHistoryRepository playerSegmentActHistoryRepository)
    {
        _playerSegmentActRepository = playerSegmentActRepository;
        _playerSegmentActHistoryRepository = playerSegmentActHistoryRepository;
    }

    public async Task CreatePlayerSegmentActAsync(PlayerSegmentAct act)
    {
        await _playerSegmentActRepository.InsertAsync(act);
    }

    public async Task CreatePlayerSegmentActHistoryAsync(PlayerSegmentActHistory actHistory)
    {
        await _playerSegmentActHistoryRepository.InsertAsync(actHistory);
    }

    public async Task CreateActWithHistoryAsync(PlayerSegmentActType action, IEnumerable<int> playerIds, string segmentId, int? byUserId, bool isBulk = default)
    {
        var act = new PlayerSegmentAct(action, playerIds.Count(), segmentId.ToLower(), byUserId);

        if (isBulk)
        {
            act.SetIsBulk();
        }

        await CreatePlayerSegmentActAsync(act);

        foreach (var playerId in playerIds)
        {
            var actHistory = new PlayerSegmentActHistory(playerId, act);
            await CreatePlayerSegmentActHistoryAsync(actHistory);
        }
    }
}