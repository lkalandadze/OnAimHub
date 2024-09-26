using OnAim.Admin.Shared.DTOs.Base;
using OnAim.Admin.Shared.Enums;

namespace OnAim.Admin.Shared.DTOs.EndpointGroup;

public record EndpointGroupFilter(
    string? Name, 
    bool? IsActive,
    HistoryStatus? HistoryStatus,
    List<int>? RoleIds, 
    List<int>? EndpointIds) : BaseFilter;
