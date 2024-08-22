using OnAim.Admin.Infrasturcture.Attributes;

namespace OnAim.Admin.Infrasturcture.Models.Request.Role
{
    public class RoleFilter
    {
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public int? PageNumber { get; set; }
        [PageSize(100)]
        public int? PageSize { get; set; }
    }
}
