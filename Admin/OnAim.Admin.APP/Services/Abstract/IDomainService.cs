using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Contracts.Dtos.EmailDomain;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Services.Abstract;

public interface IDomainService
{
    Task<ApplicationResult> CreateOrUpdateDomain(List<DomainDto>? domains, string domain, bool? isActive);
    Task<ApplicationResult> DeleteEmailDomain(List<int> domainIds);
    Task<ApplicationResult> GetAllDomain(DomainFilter filter);
    Task<AllowedEmailDomain> GetById(int id);
}
