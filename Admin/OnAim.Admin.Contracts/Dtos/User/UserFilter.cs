using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Enums;

namespace OnAim.Admin.Contracts.Dtos.User;

public class UserFilter : BaseFilter
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Direction { get; set; }
    public List<int>? RoleIds { get; set; }
    public bool? IsActive { get; set; }
    public HistoryStatus? HistoryStatus { get; set; }
    public DateTime? RegistrationDateFrom { get; set; }
    public DateTime? RegistrationDateTo { get; set; }
    public DateTime? LoginDateFrom { get; set; }
    public DateTime? LoginDateTo { get; set; }
}
