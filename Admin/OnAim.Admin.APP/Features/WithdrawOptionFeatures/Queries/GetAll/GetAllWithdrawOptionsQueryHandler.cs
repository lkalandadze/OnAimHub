using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Withdraw;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.WithdrawOptionFeatures.Queries.GetAll;

public sealed class GetAllWithdrawOptionsQueryHandler : IQueryHandler<GetAllWithdrawOptionsQuery, ApplicationResult<PaginatedResult<WithdrawOptionDto>>>
{
    private readonly ICoinService _coinService;

    public GetAllWithdrawOptionsQueryHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult<PaginatedResult<WithdrawOptionDto>>> Handle(GetAllWithdrawOptionsQuery request, CancellationToken cancellationToken)
    {
        var res = await _coinService.GetAllWithdrawOptions(request.Filter);

        return new ApplicationResult<PaginatedResult<WithdrawOptionDto>> { Data = res.Data, Success = res.Success };
    }
}
