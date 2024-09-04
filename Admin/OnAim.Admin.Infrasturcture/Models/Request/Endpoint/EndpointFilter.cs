using OnAim.Admin.Infrasturcture.Attributes;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Infrasturcture.Models.Request.Endpoint
{
    public class EndpointFilter
    {
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsEnable { get; set; }
        public string? SortBy { get; set; }
        public bool? SortDescending { get; set; }
        public EndpointType? Type { get; set; }
        public int? PageNumber { get; set; }
        [PageSize(100)]
        public int? PageSize { get; set; }
        public List<int>? EndpointGroupIds { get; set; }
    }
    public class UpdateEndpointDto
    {
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsEnabled { get; set; }
    }
}
