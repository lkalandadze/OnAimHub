using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class LeaderboardResult : BaseEntity<int>
{
    public LeaderboardResult()
    {
        
    }
    public LeaderboardResult(int leaderboardRecordId, int playerId, string playerUsername, int placement, int amount)
    {
        LeaderboardRecordId = leaderboardRecordId;
        PlayerId = playerId;
        PlayerUsername = playerUsername;
        Placement = placement;
        Amount = amount;
    }

    public int LeaderboardRecordId { get; set; }
    public LeaderboardRecord LeaderboardRecord { get; set; }
    public int PlayerId { get; set; }
    public string PlayerUsername { get; set; }
    public int Placement { get; set; }
    public int Amount { get; set; }
}
