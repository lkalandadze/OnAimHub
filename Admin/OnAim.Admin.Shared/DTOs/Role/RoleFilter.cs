using OnAim.Admin.Shared.Attributes;

namespace OnAim.Admin.Shared.DTOs.Role
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
        public bool? IsDeleted { get; set; }
    }
}
