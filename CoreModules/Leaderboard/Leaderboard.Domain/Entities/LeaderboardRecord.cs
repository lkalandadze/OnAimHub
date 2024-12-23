using Leaderboard.Domain.Enum;
using MassTransit.Middleware;
using Shared.Domain.Entities;

namespace Leaderboard.Domain.Entities;

public class LeaderboardRecord : BaseEntity<int>
{
    public LeaderboardRecord()
    {

    }

    public LeaderboardRecord(int promotionId,
                             string promotionName,
                             string title,
                             string description,
                             EventType eventType,
                             DateTimeOffset announcementDate,
                             DateTimeOffset startDate,
                             DateTimeOffset endDate,
                             bool isGenerated,
                             string? templateId,
                             int? scheduleId,
                             Guid? correlationId)
    {
        PromotionId = promotionId;
        PromotionName = promotionName;
        Title = title;
        Description = description;
        EventType = eventType;
        AnnouncementDate = announcementDate;
        StartDate = startDate;
        EndDate = endDate;
        IsGenerated = isGenerated;
        TemplateId = templateId;
        ScheduleId = scheduleId;
        CorrelationId = correlationId;
        CreationDate = DateTimeOffset.UtcNow;
        Status = LeaderboardRecordStatus.Created;
    }
    public int PromotionId { get; set; }
    public string PromotionName { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public DateTimeOffset CreationDate { get; set; } // datetimenow
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardRecordStatus Status { get; set; }
    public bool IsGenerated { get; set; }
    public string? TemplateId { get; set; }
    public int? ScheduleId { get; set; }
    public Guid? CorrelationId { get; set; }
    public LeaderboardSchedule LeaderboardSchedule { get; set; }
    public ICollection<LeaderboardProgress> LeaderboardProgresses { get; set; } = new List<LeaderboardProgress>();
    public ICollection<LeaderboardRecordPrize> LeaderboardRecordPrizes { get; set; } = new List<LeaderboardRecordPrize>();

    public void Update(string title, string description, EventType eventType, DateTimeOffset announcementDate, DateTimeOffset startDate, DateTimeOffset endDate)
    {
        Title = title;
        Description = description;
        EventType = eventType;
        AnnouncementDate = announcementDate;
        StartDate = startDate;
        EndDate = endDate;
    }

    public void AddLeaderboardRecordPrizes(int startRank, int endRank, string coinId, int amount)
    {
        var prize = new LeaderboardRecordPrize(startRank, endRank, coinId, amount);
        LeaderboardRecordPrizes.Add(prize);
    }

    public void UpdateLeaderboardRecordPrizes(int id, int startRank, int endRank, string coinId, int amount)
    {
        var prize = LeaderboardRecordPrizes.FirstOrDefault(x => x.Id == id);

        if (prize == null) return;

        prize.Update(startRank, endRank, coinId, amount);
    }

    public void InsertProgress(int playerId, string playerUsername, int amount)
    {
        var progress = new LeaderboardProgress(playerId, playerUsername, amount);
        LeaderboardProgresses.Add(progress);
    }
}
