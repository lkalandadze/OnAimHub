using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class LeaderboardPrize : BaseEntity<int>
{
    public LeaderboardPrize(int startRank, int endRank, string prizeId, int amount) 
    {
        StartRank = startRank;
        EndRank = endRank;
        PrizeId = prizeId;
        Amount = amount;
    }
    public int StartRank { get; private set; }
    public int EndRank { get; private set; }
    public int? LeaderboardRecordId { get; private set; }
    public LeaderboardRecord? LeaderboardRecord { get; set; }
    public int? LeaderboardTemplateId { get; private set; }
    public LeaderboardTemplate? LeaderboardTemplate{ get; set; }
    public string PrizeId { get; private set; }
    public Prize Prize { get; private set; }
    public int Amount { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DateDeleted { get; private set; }

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
