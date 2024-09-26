using Leaderboard.Domain.Enum;
using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class LeaderboardRecord : BaseEntity<int>
{
    public LeaderboardRecord()
    {
        
    }

    public LeaderboardRecord(string name, DateTimeOffset announcementDate, DateTimeOffset startDate, DateTimeOffset endDate, LeaderboardType leaderboardType, JobTypeEnum jobType, int? leaderboardTemplateId)
    {
        Name = name;
        AnnouncementDate = announcementDate;
        StartDate = startDate;
        EndDate = endDate;
        LeaderboardType = leaderboardType;
        JobType = jobType;
        LeaderboardTemplateId = leaderboardTemplateId;
    }

    public string Name { get; set; }
    public int? LeaderboardTemplateId { get; set; }
    public LeaderboardTemplate LeaderboardTemplate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardType LeaderboardType { get; set; }
    public JobTypeEnum JobType { get; set; }
    public ICollection<LeaderboardPrize> LeaderboardPrizes { get; set; } = new List<LeaderboardPrize>();

    public void Update(string name, DateTimeOffset announcementDate, DateTimeOffset startDate, DateTimeOffset endDate, LeaderboardType leaderboardType, JobTypeEnum jobType)
    {
        Name = name;
        AnnouncementDate = announcementDate;
        StartDate = startDate;
        EndDate = endDate;
        LeaderboardType = leaderboardType;
        JobType = jobType;
    }

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
}
