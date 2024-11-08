using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Infrastructure.DataAccess;

namespace Leaderboard.Infrastructure.Repositories;

public class LeaderboardPrizeRepository(LeaderboardDbContext context) : BaseRepository<LeaderboardDbContext, LeaderboardPrize>(context), ILeaderboardPrizeRepository
{
}