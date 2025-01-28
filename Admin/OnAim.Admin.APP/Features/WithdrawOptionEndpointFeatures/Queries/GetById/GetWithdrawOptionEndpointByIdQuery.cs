using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Withdraw;

namespace OnAim.Admin.APP.Features.WithdrawOptionEndpointFeatures.Queries.GetById;

public record GetWithdrawOptionEndpointByIdQuery(int Id) : IQuery<ApplicationResult<WithdrawOptionEndpointDto>>;

public sealed class GetWithdrawOptionEndpointByIdQueryHandler : IQueryHandler<GetWithdrawOptionEndpointByIdQuery, ApplicationResult<WithdrawOptionEndpointDto>>
{
    private readonly ICoinService _coinService;

    public GetWithdrawOptionEndpointByIdQueryHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult<WithdrawOptionEndpointDto>> Handle(GetWithdrawOptionEndpointByIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _coinService.GetWithdrawOptionEndpointById(request.Id);

        return new ApplicationResult<WithdrawOptionEndpointDto> { Data = res.Data, Success = res.Success };
    }
}
