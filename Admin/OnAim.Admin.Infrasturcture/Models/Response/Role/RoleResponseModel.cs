using OnAim.Admin.Infrasturcture.Models.Response.EndpointGroup;

namespace OnAim.Admin.Infrasturcture.Models.Response.Role
{
    public class RoleResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<EndpointGroupModel> EndpointGroupModels { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
        public DateTimeOffset DateDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
