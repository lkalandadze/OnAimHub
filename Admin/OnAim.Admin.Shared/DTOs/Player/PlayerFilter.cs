using OnAim.Admin.Shared.DTOs.Base;

namespace OnAim.Admin.Shared.DTOs.Player
{
    public record PlayerFilter(
         string? Name,
        DateTime? DateFrom,
        DateTime? DateTo,
        List<int>? SegmentIds,
        string? Status
        ) : BaseFilter;
}
