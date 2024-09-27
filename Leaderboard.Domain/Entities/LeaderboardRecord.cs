using Leaderboard.Domain.Enum;
using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class LeaderboardRecord : BaseEntity<int>
{
    public LeaderboardRecord()
    {

    }

    public LeaderboardRecord(string name,
                             DateTimeOffset creationDate,
                             DateTimeOffset announcementDate,
                             DateTimeOffset startDate,
                             DateTimeOffset endDate,
                             LeaderboardType leaderboardType,
                             JobTypeEnum jobType,
                             int? leaderboardTemplateId,
                             LeaderboardRecordStatus status,
                             bool isGenerated)
    {
        Name = name;
        CreationDate = creationDate;
        AnnouncementDate = announcementDate;
        StartDate = startDate;
        EndDate = endDate;
        LeaderboardType = leaderboardType;
        JobType = jobType;
        LeaderboardTemplateId = leaderboardTemplateId;
        Status = status;
        IsGenerated = isGenerated;
    }

    public string Name { get; set; }
    public int? LeaderboardTemplateId { get; set; }
    public LeaderboardTemplate LeaderboardTemplate { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardType LeaderboardType { get; set; }
    public JobTypeEnum JobType { get; set; }
    public LeaderboardRecordStatus Status { get; set; }
    public bool IsGenerated { get; set; }
    public ICollection<LeaderboardProgress> LeaderboardProgresses { get; set; } = new List<LeaderboardProgress>();
    public ICollection<LeaderboardRecordPrize> LeaderboardRecordPrizes { get; set; } = new List<LeaderboardRecordPrize>();

    public void Update(string name, DateTimeOffset creationDate, DateTimeOffset announcementDate, DateTimeOffset startDate, DateTimeOffset endDate, LeaderboardType leaderboardType, JobTypeEnum jobType)
    {
        Name = name;
        CreationDate = creationDate;
        AnnouncementDate = announcementDate;
        StartDate = startDate;
        EndDate = endDate;
        LeaderboardType = leaderboardType;
        JobType = jobType;
    }

    public void AddLeaderboardRecordPrizes(int startRank, int endRank, string prizeId, int amount)
    {
        var prize = new LeaderboardRecordPrize(startRank, endRank, prizeId, amount);
        LeaderboardRecordPrizes.Add(prize);
    }

    public void UpdateLeaderboardPrizes(int id, int startRank, int endRank, string prizeId, int amount)
    {
        var prize = LeaderboardRecordPrizes.FirstOrDefault(x => x.Id == id);

        if (prize != null) return;

        prize.Update(startRank, endRank, prizeId, amount);
    }

    public void InsertProgress(int playerId, string playerUsername, int amount)
    {
        var progress = new LeaderboardProgress(playerId, playerUsername, amount);
        LeaderboardProgresses.Add(progress);
    }
}
