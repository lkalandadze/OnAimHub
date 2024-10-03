using OnAim.Admin.Shared.DTOs.Base;
using OnAim.Admin.Shared.Enums;

namespace OnAim.Admin.Shared.DTOs.EmailDomain;

public record DomainFilter(
    string? domain, 
    bool? IsActive,
    HistoryStatus? HistoryStatus
    ) : BaseFilter;
