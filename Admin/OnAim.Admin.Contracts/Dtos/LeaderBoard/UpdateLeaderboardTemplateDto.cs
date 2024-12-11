namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class UpdateLeaderboardTemplateDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public TimeSpan AnnouncementDuration { get; set; }
    public TimeSpan StartDuration { get; set; }
    public TimeSpan EndDuration { get; set; }
    public List<UpdateLeaderboardTemplateCommandCommandItem> LeaderboardPrizes { get; set; }
}
