using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.APP.Features.WithdrawOptionEndpointFeatures.Queries.GetAll;

public record GetWithdrawOptionEndpointsQuery(BaseFilter Filter) : IQuery<ApplicationResult>;

public sealed class GetWithdrawOptionEndpointsQueryHandler : IQueryHandler<GetWithdrawOptionEndpointsQuery, ApplicationResult>
{
    private readonly ICoinService _coinService;

    public GetWithdrawOptionEndpointsQueryHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult> Handle(GetWithdrawOptionEndpointsQuery request, CancellationToken cancellationToken)
    {
        var res = await _coinService.GetWithdrawOptionEndpoints(request.Filter);

        return new ApplicationResult { Data = res.Data, Success = res.Success };
    }
}
