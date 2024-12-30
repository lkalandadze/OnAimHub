namespace SagaOrchestrationStateMachine.Models;

public sealed class CreateLeaderboardRecordCommand
{
    public int PromotionId { get; set; }
    public string PromotionName { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public RepeatType RepeatType { get; set; }
    public int? RepeatValue { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardRecordStatus Status { get; set; }
    public bool IsGenerated { get; set; }
    public int? ScheduleId { get; set; }
    public string? TemplateId { get; set; }
    public int? CreatedBy { get; set; }
    public Guid CorrelationId { get; set; }
    public List<CreateLeaderboardRecordPrizeCommandItem> LeaderboardPrizes { get; set; }
}
public enum EventType
{
    Internal = 0,
    External = 1,
}
public enum RepeatType
{
    None = 0,
    EveryNDays = 1,
    DayOfWeek = 2,
    DayOfMonth = 3
}
public enum LeaderboardRecordStatus
{
    Future = 0,
    Created = 1,
    Announced = 2,
    InProgress = 3,
    Finished = 4,
    Cancelled = 5,
}
