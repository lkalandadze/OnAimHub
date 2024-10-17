using OnAim.Admin.Shared.DTOs.Segment;

namespace OnAim.Admin.Shared.DTOs.Game;

public class GameDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ConfigurationDto> Templates { get; set; }
    public List<SegmentDto> Segments { get; set; }
}
public class ConfigurationDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Name { get; set; }
    public List<SegmentDto> Segments { get; set; }
    public List<BetPricesDto> BetPrices { get; set; }
}
public class BetPricesDto
{

}
