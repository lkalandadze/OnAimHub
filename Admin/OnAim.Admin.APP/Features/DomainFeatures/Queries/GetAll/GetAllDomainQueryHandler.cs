using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.EmailDomain;
using OnAim.Admin.Shared.Enums;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.APP.Features.DomainFeatures.Queries.GetAll;

public class GetAllDomainQueryHandler : IQueryHandler<GetAllDomainQuery, ApplicationResult>
{
    private readonly IRepository<AllowedEmailDomain> _repository;

    public GetAllDomainQueryHandler(IRepository<AllowedEmailDomain> repository)
    {
        _repository = repository;
    }
    public async Task<ApplicationResult> Handle(GetAllDomainQuery request, CancellationToken cancellationToken)
    {
        var domains = _repository.Query(x =>
                     (string.IsNullOrEmpty(request.Filter.domain) || x.Domain.Contains(request.Filter.domain))).AsNoTracking();

        if (request.Filter?.HistoryStatus.HasValue == true)
        {
            switch (request.Filter.HistoryStatus.Value)
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
            request.Filter,
            domain => new DomainDto
            {
               Id = domain.Id,
               Domain = domain.Domain,
               IsActive = domain.IsActive,
               IsDeleted = domain.IsDeleted,
            },
            new List<string> { "Id", "Domain" },
            cancellationToken
        );

        return new ApplicationResult { Success = true, Data = paginatedResult };
    }
}
