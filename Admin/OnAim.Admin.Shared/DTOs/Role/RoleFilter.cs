using OnAim.Admin.Shared.DTOs.Base;
using OnAim.Admin.Shared.Enums;

namespace OnAim.Admin.Shared.DTOs.Role;

public record RoleFilter(
    string? Name, 
    bool? IsActive,
    HistoryStatus? HistoryStatus,
    List<int>? UserIds,
    List<int>? GroupIds) : BaseFilter;
