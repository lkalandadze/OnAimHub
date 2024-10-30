using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Enums;

namespace OnAim.Admin.Contracts.Dtos.Role;

public record RoleFilter(
    string? Name,
    bool? IsActive,
    HistoryStatus? HistoryStatus,
    List<int>? UserIds,
    List<int>? GroupIds) : BaseFilter;
