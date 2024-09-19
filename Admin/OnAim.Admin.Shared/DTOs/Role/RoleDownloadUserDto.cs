using OnAim.Admin.Shared.DTOs.EndpointGroup;

namespace OnAim.Admin.Shared.DTOs.Role
{
    public class RoleDownloadUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<PermissionGroupDto> PermissionGroups { get; set; }
    }
}
