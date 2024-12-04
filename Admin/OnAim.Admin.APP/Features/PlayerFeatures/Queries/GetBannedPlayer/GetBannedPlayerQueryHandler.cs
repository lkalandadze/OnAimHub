using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Player;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBannedPlayer;

public class GetBannedPlayerQueryHandler : IQueryHandler<GetBannedPlayerQuery, ApplicationResult>
{
    private readonly IPlayerService _playerService;

    public GetBannedPlayerQueryHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }
    public async Task<ApplicationResult> Handle(GetBannedPlayerQuery request, CancellationToken cancellationToken)
    {
        var result = await _playerService.GetBannedPlayer(request.Id);

        return new ApplicationResult
        {
            Success = result.Success,
            Data = result.Data,
        };
    }
}
