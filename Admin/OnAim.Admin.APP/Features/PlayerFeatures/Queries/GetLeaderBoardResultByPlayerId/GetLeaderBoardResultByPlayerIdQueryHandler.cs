using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.HubServices.Player;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetLeaderBoardResultByPlayerId;

public class GetLeaderBoardResultByPlayerIdQueryHandler : IQueryHandler<GetLeaderBoardResultByPlayerIdQuery, ApplicationResult>
{
    private readonly IPlayerService _playerService;

    public GetLeaderBoardResultByPlayerIdQueryHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }
    public async Task<ApplicationResult> Handle(GetLeaderBoardResultByPlayerIdQuery request, CancellationToken cancellationToken)
    {         
        var result = await _playerService.GetLeaderBoardResultByPlayer(request.PlayerId);

        return new ApplicationResult
        {
            Success = result.Success,
            Data = result.Data,
        };
    }
}
