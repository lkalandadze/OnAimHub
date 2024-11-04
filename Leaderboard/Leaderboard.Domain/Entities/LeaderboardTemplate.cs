using Leaderboard.Domain.Enum;
using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class LeaderboardTemplate : BaseEntity<int>
{
    public LeaderboardTemplate(string name, string description, TimeSpan startTime, int announceIn, int startIn, int endIn)
    {
        Name = name;
        Description = description;
        StartTime = startTime;
        AnnounceIn = announceIn;
        StartIn = startIn;
        EndIn = endIn;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public TimeSpan StartTime { get; set; }
    public int AnnounceIn { get; set; }
    public int StartIn { get; set; }
    public int EndIn { get; set; }
    public ICollection<LeaderboardTemplatePrize> LeaderboardTemplatePrizes { get; set; } = new List<LeaderboardTemplatePrize>();

    public void AddLeaderboardTemplatePrizes(int startRank, int endRank, string prizeId, int amount)
    {
        var prize = new LeaderboardTemplatePrize(startRank, endRank, prizeId, amount);
        LeaderboardTemplatePrizes.Add(prize);
    }

    public void UpdateLeaderboardPrizes(int id, int startRank, int endRank, string prizeId, int amount)
    {
        var prize = LeaderboardTemplatePrizes.FirstOrDefault(x => x.Id == id);

        if (prize != null) return;

        prize.Update(startRank, endRank, prizeId, amount);
    }

    public void Update(string name, string description, TimeSpan startTime, int announceIn, int startIn, int endIn)
    {
        Name = name;
        Description = description;
        StartTime = startTime;
        AnnounceIn = announceIn;
        StartIn = startIn;
        EndIn = endIn;
    }
}
