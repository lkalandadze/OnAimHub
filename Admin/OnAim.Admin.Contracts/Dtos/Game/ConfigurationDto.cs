using OnAim.Admin.Contracts.Dtos.Segment;

namespace OnAim.Admin.Contracts.Dtos.Game;

public class ConfigurationDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Name { get; set; }
    public List<SegmentDto> Segments { get; set; }
    //public List<BetPricesDto> BetPrices { get; set; }
}
