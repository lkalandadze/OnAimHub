using OnAim.Admin.Contracts.Dtos.Endpoint;
using OnAim.Admin.Contracts.Dtos.Role;
using OnAim.Admin.Contracts.Dtos.User;

namespace OnAim.Admin.Contracts.Dtos.EndpointGroup;

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
