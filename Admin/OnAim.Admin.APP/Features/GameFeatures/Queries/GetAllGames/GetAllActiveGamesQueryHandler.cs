using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.GameServices;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetAllGames;

public class GetAllActiveGamesQueryHandler : IQueryHandler<GetAllActiveGamesQuery, object>
{
    private readonly IGameService _gameService;

    public GetAllActiveGamesQueryHandler(IGameService gameService)
    {
        _gameService = gameService;
    }

    public async Task<object> Handle(GetAllActiveGamesQuery request, CancellationToken cancellationToken)
    {
        return await _gameService.GetAll(request.Filter);
    }
}
