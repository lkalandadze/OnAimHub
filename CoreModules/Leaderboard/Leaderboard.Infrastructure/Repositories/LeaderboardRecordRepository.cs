using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Infrastructure.Repositories;

public class LeaderboardRecordRepository(LeaderboardDbContext context) : BaseRepository<LeaderboardDbContext, LeaderboardRecord>(context), ILeaderboardRecordRepository
{
    public async Task<int> GetMaxExternalIdAsync(CancellationToken cancellationToken = default)
    {
        return await _context.LeaderboardRecords
            .OrderByDescending(record => record.ExternalId)
            .Select(record => record.ExternalId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}