using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Enums;

namespace OnAim.Admin.Contracts.Dtos.User;

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
