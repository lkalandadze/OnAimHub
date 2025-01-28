using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Player;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Player;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBannedPlayers;

public class GetBannedPlayersQueryHandler : IQueryHandler<GetBannedPlayersQuery, ApplicationResult<List<BannedPlayerListDto>>>
{
    private readonly IPlayerService _playerService;

    public GetBannedPlayersQueryHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }
    public async Task<ApplicationResult<List<BannedPlayerListDto>>> Handle(GetBannedPlayersQuery request, CancellationToken cancellationToken)
    {
        return await _playerService.GetAllBannedPlayers();
    }
}
