using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.WithdrawOptionFeatures.Queries.GetAll;

public sealed class GetAllWithdrawOptionsQueryHandler : IQueryHandler<GetAllWithdrawOptionsQuery, ApplicationResult>
{
    private readonly ICoinService _coinService;

    public GetAllWithdrawOptionsQueryHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult> Handle(GetAllWithdrawOptionsQuery request, CancellationToken cancellationToken)
    {
        var res = await _coinService.GetAllWithdrawOptions(request.Filter);

        return new ApplicationResult { Data = res.Data, Success = res.Success };
    }
}
