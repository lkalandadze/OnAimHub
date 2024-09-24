using OnAim.Admin.Shared.DTOs.Base;

namespace OnAim.Admin.Shared.DTOs.EndpointGroup
{
    public record EndpointGroupFilter(
        string? Name, 
        bool? IsActive, 
        List<int>? RoleIds, 
        List<int>? EndpointIds, 
        bool? IsDeleted) : BaseFilter;
}
