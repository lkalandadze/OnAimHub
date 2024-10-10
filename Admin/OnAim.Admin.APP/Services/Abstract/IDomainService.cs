using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.EmailDomain;

namespace OnAim.Admin.APP.Services.Abstract;

public interface IDomainService
{
    Task<ApplicationResult> CreateOrUpdateDomain(List<DomainDto>? domains, string domain, bool? isActive);
    Task<ApplicationResult> DeleteEmailDomain(List<int> domainIds);
    Task<ApplicationResult> GetAllDomain(DomainFilter filter);
}
