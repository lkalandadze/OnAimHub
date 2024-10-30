namespace OnAim.Admin.Contracts.Dtos.Game;

public class GameFilter
{
    public string Name { get; set; }
    public List<string> SegmentIds { get; set; }
    public string Status { get; set; }
}
