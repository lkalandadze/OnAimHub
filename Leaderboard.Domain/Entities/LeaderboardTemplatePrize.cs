using Leaderboard.Domain.Entities.Generic;

namespace Leaderboard.Domain.Entities;

public class LeaderboardTemplatePrize : BasePrize
{
    public LeaderboardTemplatePrize(int startRank, int endRank, string prizeId, int amount)
    {
        StartRank = startRank;
        EndRank = endRank;
        PrizeId = prizeId;
        Amount = amount;
    }
    public int LeaderboardTemplateId { get; set; }
    public LeaderboardTemplate LeaderboardTemplate { get; set; }

    public void Update(int startRank, int endRank, string prizeId, int amount)
    {
        StartRank = startRank;
        EndRank = endRank;
        PrizeId = prizeId;
        Amount = amount;
    }
    public void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}
