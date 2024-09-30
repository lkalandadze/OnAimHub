using OnAim.Admin.Shared.DTOs.Base;
using OnAim.Admin.Shared.Enums;

namespace OnAim.Admin.Shared.DTOs.User;

public record UserFilter(
    string? Name,
    string? Email,
    string? Direction,
    List<int>? RoleIds,
    bool? IsActive,
    HistoryStatus? HistoryStatus,
    DateTime? RegistrationDateFrom,
    DateTime? RegistrationDateTo,
    DateTime? LoginDateFrom,
    DateTime? LoginDateTo
    ) : BaseFilter;
