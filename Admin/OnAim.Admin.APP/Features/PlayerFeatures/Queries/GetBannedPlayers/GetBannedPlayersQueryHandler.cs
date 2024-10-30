using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBannedPlayers;

public class GetBannedPlayersQueryHandler : IQueryHandler<GetBannedPlayersQuery, ApplicationResult>
{
    private readonly IPlayerService _playerService;

    public GetBannedPlayersQueryHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }
    public async Task<ApplicationResult> Handle(GetBannedPlayersQuery request, CancellationToken cancellationToken)
    {
        var result = await _playerService.GetAllBannedPlayers();

        return new ApplicationResult
        {
            Success = result.Success,
            Data = result.Data
        };
    }
}
