namespace OnAim.Admin.Infrasturcture.Models.Request.Role
{
    public class UpdateRoleRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<int> EndpointGroupIds { get; set; }
    }
}
