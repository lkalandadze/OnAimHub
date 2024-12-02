namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class CreateLeaderboardRecordDto
{
    public int? LeaderboardTemplateId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    //public LeaderboardType LeaderboardType { get; set; }
    //public JobTypeEnum JobType { get; set; }
    public LeaderboardRecordStatus Status { get; set; }
    public List<CreateLeaderboardRecordPrizeCommandItem> LeaderboardPrizes { get; set; }
}
public enum EventType
{
    Internal = 0,
    External = 1,
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
public enum LeaderboardScheduleStatus
{
    Active = 0,
    Completed = 1,
    Cancelled = 2,
}
public enum RepeatType
{
    None = 0,
    EveryNDays = 1,
    DayOfWeek = 2,
    DayOfMonth = 3
}