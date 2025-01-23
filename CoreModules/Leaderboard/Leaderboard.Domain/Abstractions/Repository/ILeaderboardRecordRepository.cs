using Leaderboard.Domain.Entities;

namespace Leaderboard.Domain.Abstractions.Repository;

public interface ILeaderboardRecordRepository : IBaseRepository<LeaderboardRecord>
{
    Task<int> GetMaxExternalIdAsync(CancellationToken cancellationToken = default);
}