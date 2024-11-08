using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Infrastructure.DataAccess;

namespace Leaderboard.Infrastructure.Repositories;

public class LeaderboardScheduleRepository(LeaderboardDbContext context) : BaseRepository<LeaderboardDbContext, LeaderboardSchedule>(context), ILeaderboardScheduleRepository
{
}