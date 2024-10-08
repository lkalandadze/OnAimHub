using OnAim.Admin.Shared.DTOs.Base;

namespace OnAim.Admin.Shared.DTOs.EmailDomain;

public record DomainFilter(
    string? domain
    ) : BaseFilter;
