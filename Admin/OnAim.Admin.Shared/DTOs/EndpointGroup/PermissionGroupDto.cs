using OnAim.Admin.Shared.DTOs.Endpoint;

namespace OnAim.Admin.Shared.DTOs.EndpointGroup
{
    public class PermissionGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }
}
