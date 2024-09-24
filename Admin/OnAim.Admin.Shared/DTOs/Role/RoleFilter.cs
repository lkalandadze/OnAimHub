using OnAim.Admin.Shared.DTOs.Base;

namespace OnAim.Admin.Shared.DTOs.Role
{
    public record RoleFilter(
        string? Name, 
        bool? IsActive,
        List<int>? UserIds,
        List<int>? GroupIds, 
        bool? IsDeleted) : BaseFilter;
}
