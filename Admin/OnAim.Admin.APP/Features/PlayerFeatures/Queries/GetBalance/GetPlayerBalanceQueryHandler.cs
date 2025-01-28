using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Player;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Player;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBalance;

public class GetPlayerBalanceQueryHandler : IQueryHandler<GetPlayerBalanceQuery, ApplicationResult<List<PlayerBalanceDto>>>
{
    private readonly IPlayerService _playerService;

    public GetPlayerBalanceQueryHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    public async Task<ApplicationResult<List<PlayerBalanceDto>>> Handle(GetPlayerBalanceQuery request, CancellationToken cancellationToken)
    {
        return await _playerService.GetBalance(request.Id);
    }
}
