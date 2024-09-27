using OnAim.Admin.Shared.DTOs.Base;

namespace OnAim.Admin.Shared.DTOs.Player;

public record PlayerFilter(
    string? Name,
    DateTime? DateFrom,
    DateTime? DateTo,
    List<string>? SegmentIds,
    string? Status
    ) : BaseFilter;
