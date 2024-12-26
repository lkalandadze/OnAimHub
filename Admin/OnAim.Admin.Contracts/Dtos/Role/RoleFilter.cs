using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Enums;

namespace OnAim.Admin.Contracts.Dtos.Role;

public class RoleFilter : BaseFilter
{
    public string? Name { get; set; }
    //public int? PageNumber { get; set; }
    //public int? PageSize { get; set; }
    //public string? SortBy { get; set; }
    //public bool? SortDescending { get; set; }
    //public bool? IsActive { get; set; }
    //public HistoryStatus? HistoryStatus { get; set; }
    public List<int>? UserIds { get; set; }
    public List<int>? GroupIds { get; set; }
};
