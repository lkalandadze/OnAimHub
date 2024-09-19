using OnAim.Admin.Shared.Attributes;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Shared.DTOs.Endpoint
{
    public class EndpointFilter
    {
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public string? SortBy { get; set; }
        public bool? SortDescending { get; set; }
        public EndpointType? Type { get; set; }
        public int? PageNumber { get; set; }
        [PageSize(100)]
        public int? PageSize { get; set; }
        public List<int>? EndpointGroupIds { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
