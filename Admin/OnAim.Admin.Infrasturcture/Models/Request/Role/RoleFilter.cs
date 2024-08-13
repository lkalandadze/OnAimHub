using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Infrasturcture.Models.Request.Role
{
    public class RoleFilter
    {
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public EndpointType? Type { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
