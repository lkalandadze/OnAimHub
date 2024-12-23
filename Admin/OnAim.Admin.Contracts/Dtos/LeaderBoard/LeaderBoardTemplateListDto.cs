namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class LeaderBoardTemplateListDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public DateTime CreationDate { get; set; }
    public double AnnouncementDate { get; set; }
    public double StartDate { get; set; }
    public double EndDate { get; set; }
    public bool IsDeleted { get; set; }
    public List<leaderboardTemplatePrizesDto> LeaderboardTemplatePrizes { get; set; }
}
