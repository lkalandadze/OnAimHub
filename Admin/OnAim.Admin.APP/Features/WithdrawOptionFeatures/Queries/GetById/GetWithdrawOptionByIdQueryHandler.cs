using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.WithdrawOptionFeatures.Queries.GetById;

public sealed class GetWithdrawOptionByIdQueryHandler : IQueryHandler<GetWithdrawOptionByIdQuery, ApplicationResult>
{
    private readonly ICoinService _coinService;

    public GetWithdrawOptionByIdQueryHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult> Handle(GetWithdrawOptionByIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _coinService.GetWithdrawOptionById(request.Id);

        return new ApplicationResult { Data = res.Data, Success = res.Success };
    }
}
