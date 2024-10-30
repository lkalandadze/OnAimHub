using OnAim.Admin.Contracts.Dtos.Endpoint;

namespace OnAim.Admin.Contracts.Dtos.EndpointGroup;

public class PermissionGroupDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public List<PermissionDto> Permissions { get; set; }
}
