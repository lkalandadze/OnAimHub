using Hub.Domain.Entities;
using Hub.Domain.Entities.DbEnums;

namespace Hub.Application.Services.Abstract;

public interface IPlayerSegmentActService
{
    Task CreatePlayerSegmentActAsync(PlayerSegmentAct act);

    Task CreatePlayerSegmentActHistoryAsync(PlayerSegmentActHistory actHistory);

    Task CreateActWithHistoryAsync(PlayerSegmentActType action, IEnumerable<int> playerIds, string segmentId, int? byUserId, bool isBulk = default);
}