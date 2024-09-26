using Leaderboard.Domain.Enum;
using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class LeaderboardTemplate : BaseEntity<int>
{
    public LeaderboardTemplate(string name, JobTypeEnum jobType, TimeSpan startTime, int durationInDays, int announcementLeadTimeInDays)
    {
        Name = name;
        JobType = jobType;
        StartTime = startTime;
        DurationInDays = durationInDays;
        AnnouncementLeadTimeInDays = announcementLeadTimeInDays;
    }

    public string Name { get; set; }

    public JobTypeEnum JobType { get; set; }
    public TimeSpan StartTime { get; set; }
    public int DurationInDays { get; set; }
    public int AnnouncementLeadTimeInDays { get; set; }
    public ICollection<LeaderboardPrize> LeaderboardPrizes { get; set; } = new List<LeaderboardPrize>();

    public void AddLeaderboardPrizes(int startRank, int endRank, string prizeId, int amount)
    {
        var prize = new LeaderboardPrize(startRank, endRank, prizeId, amount);
        LeaderboardPrizes.Add(prize);
    }

    public void UpdateLeaderboardPrizes(int id, int startRank, int endRank, string prizeId, int amount)
    {
        var prize = LeaderboardPrizes.FirstOrDefault(x => x.Id == id);

        if (prize != null) return;

        prize.Update(startRank, endRank, prizeId, amount);
    }

    public void Update(string name, JobTypeEnum jobType, TimeSpan startTime, int durationInDays, int announcementLeadTimeInDays)
    {
        Name = name;
        JobType = jobType;
        StartTime = startTime;
        DurationInDays = durationInDays;
        AnnouncementLeadTimeInDays = announcementLeadTimeInDays;
    }
}
