using OnAim.Admin.Shared.DTOs.Segment;

namespace OnAim.Admin.Shared.DTOs.Game;

public class GameListDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
    public DateTime LaunchDate { get; set; }
    public List<string> Configurations { get; set; }
    public List<SegmentDto> Segments { get; set; }
}
