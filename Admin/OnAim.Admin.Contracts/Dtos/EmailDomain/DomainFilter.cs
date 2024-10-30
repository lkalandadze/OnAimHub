using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.Contracts.Dtos.EmailDomain;

public record DomainFilter(
    string? domain
    ) : BaseFilter;