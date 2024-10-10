using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBalance;

public class GetPlayerBalanceQueryHandler : IQueryHandler<GetPlayerBalanceQuery, ApplicationResult>
{
    private readonly IPlayerService _playerService;

    public GetPlayerBalanceQueryHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    public async Task<ApplicationResult> Handle(GetPlayerBalanceQuery request, CancellationToken cancellationToken)
    {
        var result = await _playerService.GetBalance(request.Id);

        return new ApplicationResult
        {
            Success = result.Success,
            Data = result.Data,
        };
    }
}
