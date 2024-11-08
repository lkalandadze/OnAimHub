using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Infrastructure.DataAccess;

namespace Leaderboard.Infrastructure.Repositories;

public class LeaderboardRecordRepository(LeaderboardDbContext context) : BaseRepository<LeaderboardDbContext, LeaderboardRecord>(context), ILeaderboardRecordRepository
{
}