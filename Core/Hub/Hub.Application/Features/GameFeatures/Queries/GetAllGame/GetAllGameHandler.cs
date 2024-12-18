using Hub.Application.Configurations;
using Hub.Application.Features.GameFeatures.Dtos;
using Hub.Application.Models.Game;
using Hub.Application.Services.Abstract;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using Shared.Lib.Extensions;
using System.Linq;

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
        var games = _gameService.GetGames();

        var allGame = new List<GameBaseDtoModel>();

        foreach (var game in games)
        {
            var endpoint = game.Address + _gameApiConfiguration.Endpoints.GetGameShortInfo;
            var gameInfo = await _httpClient.CustomGetAsync<GameShortInfoGetModel>(_gameApiConfiguration.Host, endpoint);

            if (gameInfo == null)
            {
                throw new ApiException(ApiExceptionCodeTypes.ExternalServiceError, $"Failed to retrieve info of {game.Name} game.");
            }

            allGame.Add(GameBaseDtoModel.MapFrom(game, gameInfo));
        }

        #region Filters

        //TODO: temporary, should be changed

        if (!string.IsNullOrEmpty(request.Name))
        {
            allGame = allGame.Where(g => g.Name.Contains(request.Name)).ToList();
        }

        if (request.PromotionId != null)
        {
             allGame = allGame.Where(g => g.PromotionIds.Any(pi => pi == request.PromotionId)).ToList();
        }

        #endregion

        return new GetAllGameResponse
        {
            Succeeded = true,
            Data = allGame,
        };
    }
}