using OnAim.Admin.Infrasturcture.Attributes;

namespace OnAim.Admin.Infrasturcture.Models.Request.EndpointGroup
{
    public class EndpointGroupFilter
    {
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public string? SortBy { get; set; }
        public bool? SortDescending { get; set; }
        public int? PageNumber { get; set; }
        [PageSize(100)]
        public int? PageSize { get; set; }
    }
}
