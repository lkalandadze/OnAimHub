namespace OnAim.Admin.Infrasturcture.Entities
{
    public class RoleEndpointGroup
    {
        public string RoleId { get; set; }
        public Role Role { get; set; }
        public string EndpointGroupId { get; set; }
        public EndpointGroup EndpointGroup { get; set; }
    }
}
