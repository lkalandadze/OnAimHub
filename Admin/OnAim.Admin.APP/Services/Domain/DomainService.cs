using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.APP.Services.AuthServices.Auth;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Exceptions;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.EmailDomain;
using OnAim.Admin.Shared.Enums;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.APP.Services.Domain;

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

    public async Task<ApplicationResult> CreateOrUpdateDomain(List<DomainDto>? domains, string domain, bool? isActive)
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
            return new ApplicationResult { Success = true };
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
                return new ApplicationResult { Success = true };
            }

            throw new BadRequestException("Domain Already Exists");
        }

        //Create New
        var newDomain = new AllowedEmailDomain(domain, _securityContextAccessor.UserId);
        await _repository.Store(newDomain);
        await _repository.CommitChanges();

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> DeleteEmailDomain(List<int> domainIds)
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

        return new ApplicationResult { Success = true };
    }

    public async Task<ApplicationResult> GetAllDomain(DomainFilter filter)
    {
        var domains = _repository.Query(x =>
                     (string.IsNullOrEmpty(filter.domain) || x.Domain.Contains(filter.domain))).AsNoTracking();

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

        return new ApplicationResult { Success = true, Data = paginatedResult };
    }

    public async Task<AllowedEmailDomain> GetById(int id)
    {
        var domain = await _repository.Query(x => x.Id == id).FirstOrDefaultAsync();
        return domain;
    }
}
