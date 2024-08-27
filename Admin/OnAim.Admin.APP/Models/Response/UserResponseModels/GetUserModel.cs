using OnAim.Admin.APP.Models.Response.User;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;

namespace OnAim.Admin.APP.Models.Response.UserResponseModels
{
    public class GetUserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<RoleModel> Roles { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }

    public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public List<EndpointGroupDto> EndpointGroupModels { get; set; }
    }

    public class EndpointGroupDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public List<EndpointRequestModel>? Endpoints { get; set; }
    }
}
