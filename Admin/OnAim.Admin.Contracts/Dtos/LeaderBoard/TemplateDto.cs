namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class TemplateDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public TimeSpan StartTime { get; set; }
    public int AnnounceIn { get; set; }
    public int StartIn { get; set; }
    public int EndIn { get; set; }
    public List<TemplatePrizeDto> Prizes { get; set; }
}
