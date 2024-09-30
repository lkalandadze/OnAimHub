using OnAim.Admin.Shared.DTOs.Base;
using OnAim.Admin.Shared.Enums;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Shared.DTOs.Endpoint;

public record EndpointFilter(
    string? Name, 
    bool? IsActive,
    HistoryStatus? HistoryStatus,
    EndpointType? Type, 
    List<int>? GroupIds, 
    bool? SortDescending
    ) : BaseFilter;
