using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Infrastructure.DataAccess;

namespace Leaderboard.Infrastructure.Repositories;

public class LeaderboardProgressRepository(LeaderboardDbContext context) : BaseRepository<LeaderboardDbContext, LeaderboardProgress>(context), ILeaderboardProgressRepository
{
}