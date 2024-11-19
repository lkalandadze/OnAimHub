using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.CoinFeatures.Queries.GetById;

public class GetCoinByIdQueryHandler : IQueryHandler<GetCoinByIdQuery, ApplicationResult>
{
    private readonly ICoinService _coinService;

    public GetCoinByIdQueryHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }
    public async Task<ApplicationResult> Handle(GetCoinByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _coinService.GetById(request.Id);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}
