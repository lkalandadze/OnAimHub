using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetPrizeTypes;

public class GetPrizeTypesQueryHandler : IQueryHandler<GetPrizeTypesQuery, ApplicationResult>
{
    private readonly IGameService _gameService;

    public GetPrizeTypesQueryHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<ApplicationResult> Handle(GetPrizeTypesQuery request, CancellationToken cancellationToken)
    {
        var result = await _gameService.GetPrizeTypes();

        return new ApplicationResult { Data = result };
    }
}
