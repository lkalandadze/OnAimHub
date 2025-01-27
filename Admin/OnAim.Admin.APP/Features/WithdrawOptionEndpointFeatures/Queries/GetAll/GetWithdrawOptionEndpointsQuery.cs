using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Withdraw;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.WithdrawOptionEndpointFeatures.Queries.GetAll;

public record GetWithdrawOptionEndpointsQuery(BaseFilter Filter) : IQuery<ApplicationResult<PaginatedResult<WithdrawOptionEndpointDto>>>;

public sealed class GetWithdrawOptionEndpointsQueryHandler : IQueryHandler<GetWithdrawOptionEndpointsQuery, ApplicationResult<PaginatedResult<WithdrawOptionEndpointDto>>>
{
    private readonly ICoinService _coinService;

    public GetWithdrawOptionEndpointsQueryHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult<PaginatedResult<WithdrawOptionEndpointDto>>> Handle(GetWithdrawOptionEndpointsQuery request, CancellationToken cancellationToken)
    {
        var res = await _coinService.GetWithdrawOptionEndpoints(request.Filter);

        return new ApplicationResult<PaginatedResult<WithdrawOptionEndpointDto>> { Data = res.Data, Success = res.Success };
    }
}
