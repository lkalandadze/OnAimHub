using OnAim.Admin.Contracts.Dtos.EndpointGroup;

namespace OnAim.Admin.Contracts.Dtos.Role;

public class RoleDownloadUserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public List<PermissionGroupDto> PermissionGroups { get; set; }
}
