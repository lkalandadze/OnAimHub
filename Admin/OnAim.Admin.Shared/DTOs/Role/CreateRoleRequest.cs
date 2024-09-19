namespace OnAim.Admin.Shared.DTOs.Role
{
    public class CreateRoleRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int>? EndpointGroupIds { get; set; }
    }
}
