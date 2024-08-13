using OnAim.Admin.Infrasturcture.Models.Response.EndpointGroup;

namespace OnAim.Admin.APP.Models.Response.Role
{
    public class RoleResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<EndpointGroupModel> EndpointGroupModels { get; set; }
    }
}
