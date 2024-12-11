namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class LeaderboardsPromTempdto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public DateTime CreationDate { get; set; }
    public string AnnouncementDate { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public LeaderboardRecordStatus Status { get; set; }
    public bool IsGenerated { get; set; }
    public int? ScheduleId { get; set; }
    public List<leaderboardTemplatePrizesDto> LeaderboardTemplatePrizes { get; set; }
}