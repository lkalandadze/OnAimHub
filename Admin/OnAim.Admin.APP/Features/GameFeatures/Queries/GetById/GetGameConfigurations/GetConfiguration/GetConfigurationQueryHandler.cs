using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.GameServices;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetGameConfigurations.GetConfiguration;

public class GetConfigurationQueryHandler : IQueryHandler<GetConfigurationQuery, object>
{
    private readonly IGameService _gameService;

    public GetConfigurationQueryHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<object> Handle(GetConfigurationQuery request, CancellationToken cancellationToken)
    {
        //var res = await _gameService.GetConfiguration(request.Name, request.Id);

        return "";
    }
}
