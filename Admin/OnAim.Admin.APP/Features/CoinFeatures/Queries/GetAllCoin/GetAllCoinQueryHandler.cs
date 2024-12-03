using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.CoinFeatures.Queries.GetAllCoin;

public class GetAllCoinQueryHandler : IQueryHandler<GetAllCoinQuery, ApplicationResult>
{
    private readonly ICoinTemplateService _coinService;

    public GetAllCoinQueryHandler(ICoinTemplateService coinService)
    {
        _coinService = coinService;
    }
    public async Task<ApplicationResult> Handle(GetAllCoinQuery request, CancellationToken cancellationToken)
    {
        var result = await _coinService.GetAllCoins(request.BaseFilter);

        return new ApplicationResult { Success = result.Success, Data = result.Data };  
    }
}
