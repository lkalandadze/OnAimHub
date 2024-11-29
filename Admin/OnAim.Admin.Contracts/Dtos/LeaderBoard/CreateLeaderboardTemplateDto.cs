namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class CreateLeaderboardTemplateDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public System.TimeSpan StartTime { get; set; }
    public int AnnounceIn { get; set; }
    public int StartIn { get; set; }
    public int EndIn { get; set; }
    public List<CreateLeaderboardTemplatePrizeCommandItem> LeaderboardPrizes { get; set; }
}
