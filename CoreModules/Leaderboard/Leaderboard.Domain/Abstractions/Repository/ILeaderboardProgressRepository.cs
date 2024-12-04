using Leaderboard.Domain.Entities;

namespace Leaderboard.Domain.Abstractions.Repository;

public interface ILeaderboardProgressRepository
{
    Task SaveProgressAsync(LeaderboardProgress progress, TimeSpan expiry, CancellationToken cancellationToken);
    Task<LeaderboardProgress?> GetProgressAsync(int playerId, int leaderboardRecordId, CancellationToken cancellationToken);
    Task<IEnumerable<LeaderboardProgress>> GetAllProgressAsync(int leaderboardRecordId, CancellationToken cancellationToken);
    Task DeleteProgressAsync(int playerId, int leaderboardRecordId, CancellationToken cancellationToken);
    Task ClearLeaderboardProgressAsync(int leaderboardRecordId, CancellationToken cancellationToken);
}
