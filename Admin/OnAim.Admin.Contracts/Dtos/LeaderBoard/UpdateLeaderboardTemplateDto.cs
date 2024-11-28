namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class UpdateLeaderboardTemplateDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public JobTypeEnum JobType { get; set; }
    public TimeSpan StartTime { get; set; }
    public int StartIn { get; set; }
    public int EndIn { get; set; }
    public int DurationInDays { get; set; }
    public int AnnouncementLeadTimeInDays { get; set; }
    public int CreationLeadTimeInDays { get; set; }
    public List<UpdateLeaderboardTemplateCommandCommandItem> LeaderboardPrizes { get; set; }
}
