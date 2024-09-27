namespace OnAim.Admin.Domain.Entities;

public class RoleEndpointGroup
{
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public int EndpointGroupId { get; set; }
    public EndpointGroup EndpointGroup { get; set; }
    public bool IsActive { get; set; }
}
