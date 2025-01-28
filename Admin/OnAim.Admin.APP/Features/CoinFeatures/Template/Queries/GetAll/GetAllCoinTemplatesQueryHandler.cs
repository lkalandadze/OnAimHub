using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.Dtos.Coin;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Queries.GetAll;

public sealed class GetAllCoinTemplatesQueryHandler : IQueryHandler<GetAllCoinTemplatesQuery, ApplicationResult<PaginatedResult<CoinTemplateListDto>>>
{
    private readonly ICoinTemplateService _coinTemplateService;

    public GetAllCoinTemplatesQueryHandler(ICoinTemplateService coinTemplateService)
    {
        _coinTemplateService = coinTemplateService;
    }

    public async Task<ApplicationResult<PaginatedResult<CoinTemplateListDto>>> Handle(GetAllCoinTemplatesQuery request, CancellationToken cancellationToken)
    {
        return await _coinTemplateService.GetAllCoinTemplates(request.Filter);
    }
}
