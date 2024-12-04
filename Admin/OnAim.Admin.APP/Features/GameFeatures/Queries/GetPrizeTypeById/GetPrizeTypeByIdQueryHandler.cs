using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetPrizeTypeById;

public class GetPrizeTypeByIdQueryHandler : IQueryHandler<GetPrizeTypeByIdQuery, ApplicationResult>
{
    private readonly IGameService _gameService;

    public GetPrizeTypeByIdQueryHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    public async Task<ApplicationResult> Handle(GetPrizeTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _gameService.GetPrizeTypeById(request.Id);

        return new ApplicationResult { Data = result };
    }
}
