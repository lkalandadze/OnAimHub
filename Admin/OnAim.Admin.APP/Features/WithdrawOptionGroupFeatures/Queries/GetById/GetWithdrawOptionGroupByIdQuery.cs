using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.WithdrawOptionGroupFeatures.Queries.GetById;

public record GetWithdrawOptionGroupByIdQuery(int Id) : IQuery<ApplicationResult>;

public sealed class GetWithdrawOptionGroupByIdQueryHandler : IQueryHandler<GetWithdrawOptionGroupByIdQuery, ApplicationResult>
{
    private readonly ICoinService _coinService;

    public GetWithdrawOptionGroupByIdQueryHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult> Handle(GetWithdrawOptionGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _coinService.GetWithdrawOptionGroupById(request.Id);    

        return new ApplicationResult { Data = res.Data, Success = res.Success };
    }
}
