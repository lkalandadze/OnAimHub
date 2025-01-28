using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.AdminServices.Domain;
using OnAim.Admin.Contracts.Dtos.EmailDomain;

namespace OnAim.Admin.APP.Features.DomainFeatures.Queries.GetAll;

public class GetAllDomainQueryHandler : IQueryHandler<GetAllDomainQuery, ApplicationResult<PaginatedResult<DomainDto>>>
{
    private readonly IDomainService _domainService;

    public GetAllDomainQueryHandler(IDomainService domainService)
    {
        _domainService = domainService;
    }
    public async Task<ApplicationResult<PaginatedResult<DomainDto>>> Handle(GetAllDomainQuery request, CancellationToken cancellationToken)
    {
        var list = await _domainService.GetAllDomain(request.Filter);

        return new ApplicationResult<PaginatedResult<DomainDto>>
        { 
            Success = list.Success, 
            Data = list.Data
        };
    }
}
