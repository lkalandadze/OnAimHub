using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Enums;

namespace OnAim.Admin.Contracts.Dtos.EndpointGroup;

public record EndpointGroupFilter(
    string? Name,
    bool? IsActive,
    HistoryStatus? HistoryStatus,
    List<int>? RoleIds,
    List<int>? EndpointIds) : BaseFilter;
