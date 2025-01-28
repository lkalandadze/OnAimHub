using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Contracts.Dtos.EmailDomain;
using OnAim.Admin.APP.Services.AdminServices.Domain;

namespace OnAim.Admin.APP.Services.Admin.Domain;

public class DomainService : IDomainService
{
    private readonly IRepository<AllowedEmailDomain> _repository;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public DomainService(
        IRepository<AllowedEmailDomain> repository,
        ISecurityContextAccessor SecurityContextAccessor
        )
    {
        _repository = repository;
        _securityContextAccessor = SecurityContextAccessor;
    }

    public async Task<ApplicationResult<bool>> CreateOrUpdateDomain(List<DomainDto>? domains, string domain, bool? isActive)
    {
        // Update
        if (domains != null && domains.Any())
        {
            var domainEntities = await _repository.Query(x => domains.Select(d => d.Id).Contains(x.Id)).ToListAsync();

            foreach (var domainEntity in domainEntities)
            {
                var updatedDomain = domains.First(d => d.Id == domainEntity.Id);
                domainEntity.Domain = updatedDomain.Domain;
                domainEntity.IsActive = updatedDomain.IsActive;
            }

            await _repository.CommitChanges();
            return new ApplicationResult<bool> { Success = true };
        }

        //Create If Deleted
        var existingDomain = await _repository.Query(x => x.Domain == domain).FirstOrDefaultAsync();
        if (existingDomain != null)
        {
            if (existingDomain.IsDeleted)
            {
                existingDomain.Domain = domain;
                existingDomain.IsActive = true;
                existingDomain.IsDeleted = false;
                await _repository.CommitChanges();
                return new ApplicationResult<bool> { Success = true };
            }

            throw new BadRequestException("Domain Already Exists");
        }

        //Create New
        var newDomain = new AllowedEmailDomain(domain, _securityContextAccessor.UserId);
        await _repository.Store(newDomain);
        await _repository.CommitChanges();

        return new ApplicationResult<bool> { Success = true };
    }

    public async Task<ApplicationResult<bool>> DeleteEmailDomain(List<int> domainIds)
    {
        var domainList = await _repository.Query(x => domainIds.Contains(x.Id)).ToListAsync();

        if (!domainList.Any())
            throw new NotFoundException("Domain Not Found!");

        foreach (var domain in domainList)
        {
            domain.IsActive = false;
            domain.IsDeleted = true;
        }

        await _repository.CommitChanges();

        return new ApplicationResult<bool> { Success = true };
    }

    public async Task<ApplicationResult<PaginatedResult<DomainDto>>> GetAllDomain(DomainFilter filter)
    {
        var domains = _repository.Query(x =>
                     string.IsNullOrEmpty(filter.domain) || x.Domain.Contains(filter.domain)).AsNoTracking();

        if (filter?.HistoryStatus.HasValue == true)
        {
            switch (filter.HistoryStatus.Value)
            {
                case HistoryStatus.Existing:
                    domains = domains.Where(u => u.IsDeleted == false);
                    break;
                case HistoryStatus.Deleted:
                    domains = domains.Where(u => u.IsDeleted == true);
                    break;
                case HistoryStatus.All:
                    break;
                default:
                    break;
            }
        }

        bool sortDescending = filter.SortDescending.GetValueOrDefault();
        if (filter.SortBy == "Id" || filter.SortBy == "id")
        {
            domains = sortDescending
                ? domains.OrderByDescending(x => x.Id)
                : domains.OrderBy(x => x.Id);
        }
        else if (filter.SortBy == "Domain" || filter.SortBy == "domain")
        {
            domains = sortDescending
                ? domains.OrderByDescending(x => x.Domain)
                : domains.OrderBy(x => x.Domain);
        }

        var paginatedResult = await Paginator.GetPaginatedResult(
            domains,
            filter,
            domain => new DomainDto
            {
                Id = domain.Id,
                Domain = domain.Domain,
                IsActive = domain.IsActive,
                IsDeleted = domain.IsDeleted,
            },
            new List<string> { "Id", "Domain" }
        );

        return new ApplicationResult<PaginatedResult<DomainDto>> { Success = true, Data = paginatedResult };
    }

    public async Task<AllowedEmailDomain> GetById(int id)
    {
        var domain = await _repository.Query(x => x.Id == id).FirstOrDefaultAsync();
        return domain;
    }
}
