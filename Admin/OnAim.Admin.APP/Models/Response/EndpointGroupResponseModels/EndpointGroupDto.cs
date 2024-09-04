using OnAim.Admin.APP.Models.Response.EndpointModels;
using OnAim.Admin.APP.Models.Response.User;

namespace OnAim.Admin.APP.Models.Response.EndpointGroupResponseModels
{
    public class EndpointGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<EndpointModel> Endpoints { get; set; }
        public List<RoleDto> Roles { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateDeleted { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
    }
}
