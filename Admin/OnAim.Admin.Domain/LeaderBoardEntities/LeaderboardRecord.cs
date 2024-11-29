using OfficeOpenXml.Drawing;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Domain.HubEntities;

namespace OnAim.Admin.Domain.LeaderBoradEntities;

public class LeaderboardRecord : BaseEntity<int>
{
    public int PromotionId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public DateTimeOffset CreationDate { get; set; } // datetimenow
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardRecordStatus Status { get; set; }
    public bool IsGenerated { get; set; }
    public int? TemplateId { get; set; }
    public int? ScheduleId { get; set; }
    public Guid? CorrelationId { get; set; }
    public ICollection<LeaderboardProgress> LeaderboardProgresses { get; set; } = new List<LeaderboardProgress>();
    public ICollection<LeaderboardRecordPrize> LeaderboardRecordPrizes { get; set; } = new List<LeaderboardRecordPrize>();
}
public enum EventType
{
    Internal = 0,
    External = 1,
}
