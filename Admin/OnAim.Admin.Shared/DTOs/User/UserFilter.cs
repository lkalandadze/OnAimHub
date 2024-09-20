using OnAim.Admin.Shared.Attributes;

namespace OnAim.Admin.Shared.DTOs.User
{
    public record UserFilter(
        string? Name,
        string? Email,
        string? SortBy,
        bool? SortDescending,
        int? PageNumber,
        [PageSize(100)]
        int? PageSize,
        string? Direction,
        List<int>? RoleIds,
        bool? IsActive,
        bool? IsDeleted,
        DateTime? RegistrationDateFrom,
        DateTime? RegistrationDateTo,
        DateTime? LoginDateFrom,
        DateTime? LoginDateTo
        );
}
