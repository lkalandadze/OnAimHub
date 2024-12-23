namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class CreateLeaderboardTemplateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public long AnnouncementDuration { get; set; }
    public long StartDuration { get; set; }
    public long EndDuration { get; set; }
    public List<CreateLeaderboardTemplatePrizeCommandItem> LeaderboardPrizes { get; set; }
}
