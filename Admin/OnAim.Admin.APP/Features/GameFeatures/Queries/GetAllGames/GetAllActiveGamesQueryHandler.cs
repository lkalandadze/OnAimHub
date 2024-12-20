using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetAllGames;

public class GetAllActiveGamesQueryHandler : IQueryHandler<GetAllActiveGamesQuery, ApplicationResult>
{
    private readonly IGameService _gameService;

    public GetAllActiveGamesQueryHandler(IGameService gameService)
    {
        _gameService = gameService;
    }

    public async Task<ApplicationResult> Handle(GetAllActiveGamesQuery request, CancellationToken cancellationToken)
    {

        var result = await _gameService.GetAll(request.Filter);

        return new ApplicationResult { Data = result };
    }
}
