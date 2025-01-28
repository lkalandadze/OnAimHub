using AggregationService.Domain.Entities;

namespace OnAim.Admin.Domain.LeaderBoradEntities;

public class LeaderboardRecord
{
    public LeaderboardRecord()
    {
        
    }

    public LeaderboardRecord(int promotionId,
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
        CreationDate = DateTime.UtcNow;
        Status = LeaderboardRecordStatus.Created;
    }
    public int Id { get; set; }
    public int PromotionId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardRecordStatus Status { get; set; }
    public bool IsGenerated { get; set; }
    public string? TemplateId { get; set; }
    public int? ScheduleId { get; set; }
    public int? CreatedBy { get; set; }
    public Guid? CorrelationId { get; set; }
    public LeaderboardSchedule LeaderboardSchedule { get; set; }
    public ICollection<LeaderboardProgress> LeaderboardProgresses { get; set; } = new List<LeaderboardProgress>();
    public ICollection<LeaderboardRecordPrize> LeaderboardRecordPrizes { get; set; } = new List<LeaderboardRecordPrize>();
    public ICollection<AggregationConfiguration> AggregationConfigurations { get; set; } = new List<AggregationConfiguration>();
    public void AddLeaderboardRecordPrizes(int id, int startRank, int endRank, string coinId, int amount)
    {
        var prize = new LeaderboardRecordPrize(id, startRank, endRank, coinId, amount);
        LeaderboardRecordPrizes.Add(prize);
    }
}