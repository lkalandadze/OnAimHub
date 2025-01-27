using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Coin;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Withdraw;

namespace OnAim.Admin.APP.Features.WithdrawOptionGroupFeatures.Queries.GetById;

public record GetWithdrawOptionGroupByIdQuery(int Id) : IQuery<ApplicationResult<WithdrawOptionGroupDto>>;

public sealed class GetWithdrawOptionGroupByIdQueryHandler : IQueryHandler<GetWithdrawOptionGroupByIdQuery, ApplicationResult<WithdrawOptionGroupDto>>
{
    private readonly ICoinService _coinService;

    public GetWithdrawOptionGroupByIdQueryHandler(ICoinService coinService)
    {
        _coinService = coinService;
    }

    public async Task<ApplicationResult<WithdrawOptionGroupDto>> Handle(GetWithdrawOptionGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _coinService.GetWithdrawOptionGroupById(request.Id);    

        return new ApplicationResult<WithdrawOptionGroupDto> { Data = res.Data, Success = res.Success };
    }
}
