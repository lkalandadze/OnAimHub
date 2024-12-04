using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class LeaderboardProgress
{
    public LeaderboardProgress(int playerId, string playerUsername, int amount)
    {
        PlayerId = playerId;
        PlayerUsername = playerUsername;
        Amount = amount;
    }

    public int LeaderboardRecordId { get; set; }
    public int PlayerId { get; set; }
    public string PlayerUsername { get; set; }
    public int Amount { get; set; }
}
