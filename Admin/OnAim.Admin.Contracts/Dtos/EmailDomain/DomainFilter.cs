using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.Contracts.Dtos.EmailDomain;

public class DomainFilter : BaseFilter
{ 
    public string? domain { get; set; }
}
