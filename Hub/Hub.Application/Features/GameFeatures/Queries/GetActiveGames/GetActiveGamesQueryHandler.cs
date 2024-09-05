using Hub.Application.Services.Abstract;
using MediatR;
using Shared.Lib.Wrappers;

namespace Hub.Application.Features.GameFeatures.Queries.GetActiveGames;

public class GetActiveGamesQueryHandler : IRequestHandler<GetActiveGamesQuery, List<Response<GetActiveGamesQueryResponse>>>
{
    private readonly IActiveGameService _activeGameService;
    private readonly IAuthService _authService;
    public GetActiveGamesQueryHandler(IActiveGameService activeGameService, IAuthService authService)
    {
        _activeGameService = activeGameService;
        _authService = authService;
    }

    public async Task<List<Response<GetActiveGamesQueryResponse>>> Handle(GetActiveGamesQuery query, CancellationToken cancellationToken)
    {
        var userSegmentIds = _authService.GetCurrentPlayerSegmentIds();

        var activeGames = _activeGameService.GetActiveGames();

        if (activeGames == null || !activeGames.Any())
        {
            return new List<Response<GetActiveGamesQueryResponse>>
            {
                new Response<GetActiveGamesQueryResponse>("No active games found.")
            };
        }

        var filteredGames = activeGames
            .Where(game => game.SegmentIds.Any(segmentId => userSegmentIds.Contains(segmentId)))
            .Select(game => new Response<GetActiveGamesQueryResponse>(new GetActiveGamesQueryResponse(
                game.Id,
                game.Name,
                game.Address)))
            .ToList();

        return filteredGames;
    }
}
