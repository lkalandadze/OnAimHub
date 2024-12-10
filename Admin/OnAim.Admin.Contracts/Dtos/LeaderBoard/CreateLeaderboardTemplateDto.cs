namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class CreateLeaderboardTemplateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public DateTimeOffset AnnouncementDuration { get; set; }
    public DateTimeOffset StartDuration { get; set; }
    public DateTimeOffset EndDuration { get; set; }
    public List<CreateLeaderboardTemplatePrizeCommandItem> LeaderboardPrizes { get; set; }
}
