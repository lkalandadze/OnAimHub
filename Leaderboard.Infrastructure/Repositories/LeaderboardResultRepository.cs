using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Infrastructure.DataAccess;

namespace Leaderboard.Infrastructure.Repositories;

public class LeaderboardResultRepository(LeaderboardDbContext context) : BaseRepository<LeaderboardDbContext, LeaderboardResult>(context), ILeaderboardResultRepository
{
}