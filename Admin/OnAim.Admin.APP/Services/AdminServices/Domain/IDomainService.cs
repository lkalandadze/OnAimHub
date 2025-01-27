using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Contracts.Dtos.EmailDomain;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Services.AdminServices.Domain;

public interface IDomainService
{
    Task<ApplicationResult<bool>> CreateOrUpdateDomain(List<DomainDto>? domains, string domain, bool? isActive);
    Task<ApplicationResult<bool>> DeleteEmailDomain(List<int> domainIds);
    Task<ApplicationResult<PaginatedResult<DomainDto>>> GetAllDomain(DomainFilter filter);
    Task<AllowedEmailDomain> GetById(int id);
}
