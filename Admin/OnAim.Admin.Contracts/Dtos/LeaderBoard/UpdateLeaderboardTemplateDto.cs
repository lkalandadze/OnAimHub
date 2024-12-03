namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class UpdateLeaderboardTemplateDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public List<UpdateLeaderboardTemplateCommandCommandItem> LeaderboardPrizes { get; set; }
}
