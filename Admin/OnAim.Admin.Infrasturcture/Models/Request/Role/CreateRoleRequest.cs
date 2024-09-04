using OnAim.Admin.Infrasturcture.Models.Request.EndpointGroup;

namespace OnAim.Admin.Infrasturcture.Models.Request.Role
{
    public class CreateRoleRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int>? EndpointGroupIds { get; set; }
    }
}
