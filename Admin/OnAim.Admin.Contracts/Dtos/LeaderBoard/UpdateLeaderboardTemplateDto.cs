namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class UpdateLeaderboardTemplateDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public JobTypeEnum JobType { get; set; }
    public TimeSpan StartTime { get; set; }
    public int DurationInDays { get; set; }
    public int AnnouncementLeadTimeInDays { get; set; }
    public int CreationLeadTimeInDays { get; set; }
    public List<UpdateLeaderboardTemplateCommandCommandItem> LeaderboardPrizes { get; set; }
}
