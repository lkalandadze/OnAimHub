using OnAim.Admin.Contracts.Dtos.Segment;

namespace OnAim.Admin.Contracts.Dtos.Game;

public class GameDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ConfigurationDto> Templates { get; set; }
    public List<SegmentDto> Segments { get; set; }
}
