using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

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

        var result = await _gameService.GetAll();

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}
