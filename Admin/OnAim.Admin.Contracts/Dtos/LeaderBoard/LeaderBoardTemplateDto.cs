namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class LeaderBoardTemplateDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int AnnounceIn {  get; set; }
    public System.TimeSpan StartTime {  get; set; }
    public int StartIn { get; set; }
    public int EndIn { get; set; }
    public int Segments { get; set; }
    public int Usage { get; set; }
}
