using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.Contracts.Dtos.Segment;

public class SegmentActsFilter : BaseFilter
{
    public int? UserId { get; set; }
    public string? SegmentId { get; set; }
    public DateTimeOffset? DateFrom { get; set; }
    public DateTimeOffset? DateTo { get; set; }
}
