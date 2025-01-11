using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.APP.Features.WithdrawOptionGroupFeatures.Queries.GetAll;

public record GetAllWithdrawOptionGroupsQuery(BaseFilter Filter) : IQuery<ApplicationResult>;

public sealed class GetAllWithdrawOptionGroupsQueryHandler : IQueryHandler<GetAllWithdrawOptionGroupsQuery, ApplicationResult>
{
    private readonly ICoinService _coinService;

    public GetAllWithdrawOptionGroupsQueryHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult> Handle(GetAllWithdrawOptionGroupsQuery request, CancellationToken cancellationToken)
    {
        var res = await _coinService.GetAllWithdrawOptionGroups(request.Filter);

        return new ApplicationResult { Data = res.Data, Success = res.Success };
    }
}
