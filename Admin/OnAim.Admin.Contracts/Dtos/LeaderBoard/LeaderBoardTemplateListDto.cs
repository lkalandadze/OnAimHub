namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class LeaderBoardTemplateListDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public string AnnouncementDate { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public bool IsDeleted { get; set; }
    public List<leaderboardTemplatePrizesDto> LeaderboardTemplatePrizes { get; set; }
}
