using Hub.Application.Services.Abstract;
using MediatR;

namespace Hub.Application.Features.GameFeatures.Queries.GetActiveGames;

public class GetActiveGamesQueryHandler : IRequestHandler<GetActiveGamesQuery, List<GetActiveGamesQueryResponse>>
{
    private readonly IActiveGameService _activeGameService;
    private readonly IAuthService _authService;
    public GetActiveGamesQueryHandler(IActiveGameService activeGameService, IAuthService authService)
    {
        _activeGameService = activeGameService;
        _authService = authService;
    }

    public async Task<List<GetActiveGamesQueryResponse>> Handle(GetActiveGamesQuery query, CancellationToken cancellationToken)
    {
        var userSegmentIds = _authService.GetCurrentPlayerSegmentIds();

        // Filter active games based on segment IDs
        var filteredGames = _activeGameService.GetActiveGames()
            .Where(game => game.SegmentIds.Any(segmentId => userSegmentIds.Contains(segmentId)))
            .Select(game => new GetActiveGamesQueryResponse
            {
                Id = game.Id,
                Name = game.Name,
                Address = game.Address
            })
            .ToList();

        return filteredGames;
    }
}
