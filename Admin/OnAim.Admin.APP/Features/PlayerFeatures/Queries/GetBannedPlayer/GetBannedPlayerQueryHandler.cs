using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Player;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Domain.HubEntities.PlayerEntities;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBannedPlayer;

public class GetBannedPlayerQueryHandler : IQueryHandler<GetBannedPlayerQuery, ApplicationResult<PlayerBan>>
{
    private readonly IPlayerService _playerService;

    public GetBannedPlayerQueryHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }
    public async Task<ApplicationResult<PlayerBan>> Handle(GetBannedPlayerQuery request, CancellationToken cancellationToken)
    {
        return await _playerService.GetBannedPlayer(request.Id);
    }
}
