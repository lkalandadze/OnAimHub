namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class UpdateLeaderboardRecordDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset AnnouncementDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    //public LeaderboardType LeaderboardType { get; set; }
    //public JobTypeEnum JobType { get; set; }
    public List<UpdateLeaderboardRecordCommandItem> Prizes { get; set; }
}
