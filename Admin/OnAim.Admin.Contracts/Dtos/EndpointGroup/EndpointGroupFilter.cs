using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Enums;

namespace OnAim.Admin.Contracts.Dtos.EndpointGroup;

public class EndpointGroupFilter : BaseFilter
{
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
    public HistoryStatus? HistoryStatus { get; set; }
    public List<int>? RoleIds { get; set; }
    public List<int>? EndpointIds { get; set; }
}
