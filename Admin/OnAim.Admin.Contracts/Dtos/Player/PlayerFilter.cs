using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.Contracts.Dtos.Player;

public class PlayerFilter : BaseFilter
{
    public string? Name { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public List<string>? SegmentIds { get; set; }
    public bool? IsBanned { get; set; }
}
