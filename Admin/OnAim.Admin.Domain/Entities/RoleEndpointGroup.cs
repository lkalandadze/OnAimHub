namespace OnAim.Admin.Domain.Entities;

public class RoleEndpointGroup
{
    public RoleEndpointGroup(int roleId, int endpointGroupId)
    {
        RoleId = roleId;
        EndpointGroupId = endpointGroupId;
        IsActive = true;
    }

    public int RoleId { get; set; }
    public Role Role { get; set; }
    public int EndpointGroupId { get; set; }
    public EndpointGroup EndpointGroup { get; set; }
    public bool IsActive { get; set; }
}
