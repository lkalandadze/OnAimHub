using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.DomainFeatures.Queries.GetAll;

public class GetAllDomainQueryHandler : IQueryHandler<GetAllDomainQuery, ApplicationResult>
{
    private readonly IDomainService _domainService;

    public GetAllDomainQueryHandler(IDomainService domainService)
    {
        _domainService = domainService;
    }
    public async Task<ApplicationResult> Handle(GetAllDomainQuery request, CancellationToken cancellationToken)
    {
        var list = await _domainService.GetAllDomain(request.Filter);

        return new ApplicationResult { Success = true, Data = list };
    }
}
