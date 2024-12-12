using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Infrastructure.DataAccess;

namespace Leaderboard.Infrastructure.Repositories;

public class LeaderboardRecordPrizeRepository(LeaderboardDbContext context) : BaseRepository<LeaderboardDbContext, LeaderboardRecordPrize>(context), ILeaderboardRecordPrizeRepository
{
}