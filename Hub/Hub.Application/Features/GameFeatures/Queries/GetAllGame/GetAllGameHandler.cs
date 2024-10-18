using Hub.Application.Configurations;
using Hub.Application.Features.GameFeatures.Dtos;
using Hub.Application.Models.Game;
using Hub.Application.Services.Abstract;
using MediatR;
using Microsoft.Extensions.Options;

namespace Hub.Application.Features.GameFeatures.Queries.GetAllGame;

public class GetAllGameHandler : IRequestHandler<GetAllGameQuery, GetAllGameResponse>
{
    private readonly IAuthService _authService;
    private readonly IGameService _gameService;
    private readonly HttpClient _httpClient;
    private readonly GameApiConfiguration _gameApiConfiguration;

    public GetAllGameHandler(IAuthService authService, IGameService gameService, HttpClient httpClient, IOptions<GameApiConfiguration> gameApiConfiguration)
    {
        _authService = authService;
        _gameService = gameService;
        _httpClient = httpClient;
        _gameApiConfiguration = gameApiConfiguration.Value;
    }

    public async Task<GetAllGameResponse> Handle(GetAllGameQuery request, CancellationToken cancellationToken)
    {
        //var games = _gameService.GetGames();

        var games = new List<GameModel>
        {
            new GameModel { Name = "WheelApi", Address = "WheelApi" },
        };

        var allGame = new List<GameBaseDtoModel>();

        foreach (var game in games)
        {
            var endpoint = _gameApiConfiguration.Host + game.Address + _gameApiConfiguration.Endpoints.GetGameConfigurations;
        }


        //should call each game to get:
        //segments, configurations count, description and status

        //if (!string.IsNullOrEmpty(request.Name))
        //{
        //    games = games.Where(g => g.Name.Contains(request.Name));
        //}

        //if(request.SegmentIds != null && request.SegmentIds.Any())
        //{
        //    games = games.Where(g => g.SegmentIds.Any(segmentId => request.SegmentIds.Contains(segmentId)));
        //}

        return new GetAllGameResponse
        {
            Succeeded = true,
            Data = games.Select(x => GameBaseDtoModel.MapFrom(x)),
        };
    }
}