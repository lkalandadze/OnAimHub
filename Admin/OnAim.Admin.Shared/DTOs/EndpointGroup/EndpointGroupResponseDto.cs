using OnAim.Admin.Shared.DTOs.Endpoint;
using OnAim.Admin.Shared.DTOs.Role;

namespace OnAim.Admin.Shared.DTOs.EndpointGroup
{
    public class EndpointGroupResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<EndpointModel> Endpoints { get; set; }
        public List<RoleDto> Roles { get; set; }
        public UserDto? CreatedBy { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateDeleted { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
    }
}
