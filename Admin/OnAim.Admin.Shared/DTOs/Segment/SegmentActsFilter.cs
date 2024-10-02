using OnAim.Admin.Shared.DTOs.Base;

namespace OnAim.Admin.Shared.DTOs.Segment
{
    public record SegmentActsFilter(
        int? UserId, 
        int? playerId,
        string? SegmentId, 
        DateTimeOffset? DateFrom, 
        DateTimeOffset? DateTo
        ) : BaseFilter;
}
