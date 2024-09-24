using OnAim.Admin.Shared.DTOs.Base;

namespace OnAim.Admin.Shared.DTOs.User
{
    public record UserFilter(
        string? Name,
        string? Email,
        string? Direction,
        List<int>? RoleIds,
        bool? IsActive,
        bool? IsExisted,
        bool? IsDeleted,
        DateTime? RegistrationDateFrom,
        DateTime? RegistrationDateTo,
        DateTime? LoginDateFrom,
        DateTime? LoginDateTo
        ) : BaseFilter;
}
