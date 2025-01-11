using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.WithdrawOptionEndpointFeatures.Queries.GetById;

public record GetWithdrawOptionEndpointByIdQuery(int Id) : IQuery<ApplicationResult>;

public sealed class GetWithdrawOptionEndpointByIdQueryHandler : IQueryHandler<GetWithdrawOptionEndpointByIdQuery, ApplicationResult>
{
    private readonly ICoinService _coinService;

    public GetWithdrawOptionEndpointByIdQueryHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult> Handle(GetWithdrawOptionEndpointByIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _coinService.GetWithdrawOptionEndpointById(request.Id);

        return new ApplicationResult { Data = res.Data, Success = res.Success };
    }
}
