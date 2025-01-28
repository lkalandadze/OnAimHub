using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.GameServices;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetById;

public class GetGameQueryHandler : IQueryHandler<GetGameQuery, object>
{
    private readonly IGameService _gameService;

    public GetGameQueryHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<object> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        return await _gameService.GetGame(request.name);
    }
}
