using OnAim.Admin.Shared.DTOs.EndpointGroup;

namespace OnAim.Admin.Shared.DTOs.Role
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public List<EndpointGroupDto> EndpointGroupModels { get; set; }
    }
}
