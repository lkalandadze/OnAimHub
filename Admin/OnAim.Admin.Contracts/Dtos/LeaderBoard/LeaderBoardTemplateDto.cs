namespace OnAim.Admin.Contracts.Dtos.LeaderBoard;

public class LeaderBoardTemplateDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Announce {  get; set; }
    public TimeSpan Start {  get; set; }
    public TimeSpan End { get; set; }
    public int Segments { get; set; }
    public int Usage { get; set; }
}
