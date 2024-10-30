using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.Contracts.Dtos.Player;

public record PlayerFilter(
    string? Name,
    DateTime? DateFrom,
    DateTime? DateTo,
    List<string>? SegmentIds,
    bool? IsBanned
    ) : BaseFilter;
