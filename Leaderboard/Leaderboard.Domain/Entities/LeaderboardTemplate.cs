using Leaderboard.Domain.Enum;
using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class LeaderboardTemplate : BaseEntity<int>
{
    public LeaderboardTemplate(string name, JobTypeEnum jobType, TimeSpan startTime, int durationInDays, int announcementLeadTimeInDays, int creationLeadTimeInDays)
    {
        Name = name;
        JobType = jobType;
        StartTime = startTime;
        DurationInDays = durationInDays;
        AnnouncementLeadTimeInDays = announcementLeadTimeInDays;
        CreationLeadTimeInDays = creationLeadTimeInDays;
    }

    public string Name { get; set; }

    public JobTypeEnum JobType { get; set; }
    public TimeSpan StartTime { get; set; }
    public int DurationInDays { get; set; }
    public int AnnouncementLeadTimeInDays { get; set; }
    public int CreationLeadTimeInDays { get; set; }
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

    public void Update(string name, JobTypeEnum jobType, TimeSpan startTime, int durationInDays, int announcementLeadTimeInDays, int creationgLeadTimeInDays)
    {
        Name = name;
        JobType = jobType;
        StartTime = startTime;
        DurationInDays = durationInDays;
        AnnouncementLeadTimeInDays = announcementLeadTimeInDays;
        CreationLeadTimeInDays = creationgLeadTimeInDays;
    }
}
