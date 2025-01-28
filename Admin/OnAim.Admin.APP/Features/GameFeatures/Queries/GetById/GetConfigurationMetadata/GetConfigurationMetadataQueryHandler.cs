using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.GameServices;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetConfigurationMetadata;

public class GetConfigurationMetadataQueryHandler : IQueryHandler<GetConfigurationMetadataQuery, object>
{
    private readonly IGameService _gameService;

    public GetConfigurationMetadataQueryHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<object> Handle(GetConfigurationMetadataQuery request, CancellationToken cancellationToken)
    {
        return await _gameService.GetConfigurationMetadata(request.Name);
    }
}
