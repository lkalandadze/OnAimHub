using Hub.Application.Configurations;
using Hub.Application.Features.GameFeatures.Dtos;
using Hub.Application.Models.Game;
using Hub.Application.Services.Abstract;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Lib.Extensions;

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

        var allGame = new List<GameBaseDtoModel>();

        foreach (var game in games)
        {
            var endpoint = game.Address + _gameApiConfiguration.Endpoints.GetGameConfigurations;
            var gameConfigurations = await _httpClient.CustomGetAsync<IEnumerable<GameConfigurationGetModel>>(_gameApiConfiguration.Host, endpoint);

            if (gameConfigurations == null || !gameConfigurations.Any())
            {
                throw new ApiException(ApiExceptionCodeTypes.ExternalServiceError, $"Failed to retrieve configurations from {game.Name}.");
            }

            allGame.Add(GameBaseDtoModel.MapFrom(game, gameConfigurations));
        }

        if (request.IsAuthorized)
        {
            var playerSegments = _authService.GetCurrentPlayerSegments().Select(ps => ps.SegmentId);

            allGame = allGame.Where(game => game.Configurations
                             .Any(config => config.Segments
                             .Any(segment => playerSegments.Contains(segment.Id))))
                             .ToList();
        }

        return new GetAllGameResponse
        {
            Succeeded = true,
            Data = allGame
        };
    }
}