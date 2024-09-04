using OnAim.Admin.Infrasturcture.Attributes;

namespace OnAim.Admin.Infrasturcture.Models.Request.Role
{
    public class RoleFilter
    {
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public string? SortBy { get; set; }
        public bool? SortDescending { get; set; }
        public int? PageNumber { get; set; }
        [PageSize(100)]
        public int? PageSize { get; set; }
        public List<int>? UserIds { get; set; }
        public List<int>? GroupIds { get; set; }
    }
}
