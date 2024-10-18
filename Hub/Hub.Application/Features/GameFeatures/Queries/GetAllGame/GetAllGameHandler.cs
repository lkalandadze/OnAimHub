using Hub.Application.Configurations;
using Hub.Application.Features.GameFeatures.Dtos;
using Hub.Application.Services.Abstract;
using MediatR;
using Microsoft.Extensions.Options;

namespace Hub.Application.Features.GameFeatures.Queries.GetAllGame;

public class GetAllGameHandler : IRequestHandler<GetAllGameQuery, GetAllGameResponse>
{
    private readonly IAuthService _authService;
    private readonly IActiveGameService _gameService;
    private readonly HttpClient _httpClient;
    private readonly GameApiConfiguration _gameApiConfiguration;

    public GetAllGameHandler(IAuthService authService, IActiveGameService gameService, HttpClient httpClient, IOptions<GameApiConfiguration> gameApiConfiguration)
    {
        _authService = authService;
        _gameService = gameService;
        _httpClient = httpClient;
        _gameApiConfiguration = gameApiConfiguration.Value;
    }

    public async Task<GetAllGameResponse> Handle(GetAllGameQuery request, CancellationToken cancellationToken)
    {
        var games = _gameService.GetActiveGames();

        if (!string.IsNullOrEmpty(request.Name))
        {
            games = games.Where(g => g.Name.Contains(request.Name));
        }

        if(request.SegmentIds != null && request.SegmentIds.Any())
        {
            games = games.Where(g => g.SegmentIds.Any(segmentId => request.SegmentIds.Contains(segmentId)));
        }

        return new GetAllGameResponse
        {
            Succeeded = true,
            Data = games.Select(x => GameBaseDtoModel.MapFrom(x)),
        };
    }
}