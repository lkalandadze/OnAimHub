using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.Contracts.Dtos.Segment;

public record SegmentActsFilter(
    int? UserId,
    //int? playerId,
    string? SegmentId,
    DateTimeOffset? DateFrom,
    DateTimeOffset? DateTo
    ) : BaseFilter;
