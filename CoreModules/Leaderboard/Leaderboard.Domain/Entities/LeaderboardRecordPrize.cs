using Leaderboard.Domain.Entities.Generic;

namespace Leaderboard.Domain.Entities;

public class LeaderboardRecordPrize : BasePrize
{
    public LeaderboardRecordPrize(int startRank, int endRank, string coinId, int amount)
    {
        StartRank = startRank;
        EndRank = endRank;
        CoinId = coinId;
        Amount = amount;
    }
    public int LeaderboardRecordId { get; set; }
    public LeaderboardRecord LeaderboardRecord { get; set; }

    public void Update(int startRank, int endRank, string coinId, int amount)
    {
        StartRank = startRank;
        EndRank = endRank;
        CoinId = coinId;
        Amount = amount;
    }
    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}
