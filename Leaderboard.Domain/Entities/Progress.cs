using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class Progress : BaseEntity<int>
{
    public int PlayerId { get; set; }
    public int CurrentRank { get; set; }
    public int Amount { get; set; }
}
