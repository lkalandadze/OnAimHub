using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Hub.Player;
using OnAim.Admin.APP.Services.HubServices.Player;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetLeaderBoardResultByPlayerId;

public class GetLeaderBoardResultByPlayerIdQueryHandler : IQueryHandler<GetLeaderBoardResultByPlayerIdQuery, UserActiveLeaderboards>
{
    private readonly IPlayerService _playerService;

    public GetLeaderBoardResultByPlayerIdQueryHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }
    public async Task<UserActiveLeaderboards> Handle(GetLeaderBoardResultByPlayerIdQuery request, CancellationToken cancellationToken)
    {         
        return await _playerService.GetLeaderBoardResultByPlayer(request.PlayerId);
    }
}
