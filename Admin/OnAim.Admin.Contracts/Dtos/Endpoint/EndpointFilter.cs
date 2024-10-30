using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Enums;
using OnAim.Admin.Contracts.Models;

namespace OnAim.Admin.Contracts.Dtos.Endpoint;

public record EndpointFilter(
    string? Name,
    bool? IsActive,
    HistoryStatus? HistoryStatus,
    EndpointType? Type,
    List<int>? GroupIds,
    bool? SortDescending
    ) : BaseFilter; 
