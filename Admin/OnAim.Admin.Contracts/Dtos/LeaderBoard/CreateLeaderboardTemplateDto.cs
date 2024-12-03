namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class CreateLeaderboardTemplateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public List<CreateLeaderboardTemplatePrizeCommandItem> LeaderboardPrizes { get; set; }
}
