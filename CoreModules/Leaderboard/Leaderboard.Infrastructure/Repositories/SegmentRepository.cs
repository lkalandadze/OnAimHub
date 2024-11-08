using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using Leaderboard.Infrastructure.DataAccess;

namespace Leaderboard.Infrastructure.Repositories;

public class SegmentRepository(LeaderboardDbContext context) : BaseRepository<LeaderboardDbContext, Segment>(context), ISegmentRepository
{
}
