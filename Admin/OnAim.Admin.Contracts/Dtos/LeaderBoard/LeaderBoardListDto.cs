namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class LeaderBoardListDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public LeaderboardRecordStatus Status { get; set; }
    public bool IsGenerated { get; set; }
    //public ICollection<PrizeDto> Prizes {  get; set; } 
    //public List<PrizeConfigurationsDto> PrizeConfigurations { get; set; }
    //public List<LeaderBoardItemsDto> LeaderBoardItems { get; set; }
}