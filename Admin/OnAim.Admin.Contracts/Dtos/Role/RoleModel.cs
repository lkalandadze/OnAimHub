using OnAim.Admin.Contracts.Dtos.EndpointGroup;

namespace OnAim.Admin.Contracts.Dtos.Role;

public class RoleModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool? IsActive { get; set; }
    public List<EndpointGroupDto> EndpointGroupModels { get; set; }
}
