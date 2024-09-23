using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class LeaderboardTemplate : BaseEntity<int>
{
    public int ConfigurationId { get; private set; }
    public Configuration Configuration { get; set; }
}
