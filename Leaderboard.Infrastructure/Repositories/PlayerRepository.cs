using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Infrastructure.DataAccess;

namespace Leaderboard.Infrastructure.Repositories;

public class PlayerRepository(LeaderboardDbContext context) : BaseRepository<LeaderboardDbContext, Player>(context), IPlayerRepository
{
}
