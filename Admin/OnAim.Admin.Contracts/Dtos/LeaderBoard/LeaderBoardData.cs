namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class LeaderBoardData
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public List<TemplatePrizeDto> Prizes { get; set; }
}
