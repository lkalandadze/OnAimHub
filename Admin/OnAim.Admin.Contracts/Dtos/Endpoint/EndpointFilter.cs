using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Enums;
using OnAim.Admin.Contracts.Models;

namespace OnAim.Admin.Contracts.Dtos.Endpoint;

public class EndpointFilter : BaseFilter
{
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
    public HistoryStatus? HistoryStatus { get; set; }
    public EndpointType? Type { get; set; }
    public List<int>? GroupIds { get; set; }
    public bool? SortDescending { get; set; }
}
