using OnAim.Admin.Contracts.Dtos.EndpointGroup;

namespace OnAim.Admin.Contracts.Dtos.Role;

public class RoleShortResponseModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public List<EndpointGroupModeldTO> EndpointGroupModels { get; set; }
}
