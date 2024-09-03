using Consul;
using GameLib.Application.Holders;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Models.Consul;
using System.Text.Json;
using Wheel.Application.Models.Game;
using Wheel.Application.Models.Player;
using Wheel.Application.Services.Abstract;
using Wheel.Domain.Entities;

namespace Wheel.Application.Services.Concrete;

public class GameService : IGameService
{
    private readonly GeneratorHolder _generatorHolder;
    private readonly ConfigurationHolder _configurationHolder;
    private readonly IAuthService _authService;
    private readonly IHubService _hubService;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IGameVersionRepository _gameVersionRepository;
    private readonly IConsulClient _consulClient;
    private readonly IConsulGameService _consulGameService;

    public GameService(
        GeneratorHolder generatorHolder,
        ConfigurationHolder configurationHolder,
        IAuthService authService,
        IHubService hubService,
        IConfigurationRepository configurationRepository,
        IGameVersionRepository gameVersionRepository,
        IConsulClient consulClient,
        IConsulGameService consulGameService)
    {
        _generatorHolder = generatorHolder;
        _configurationHolder = configurationHolder;
        _authService = authService;
        _hubService = hubService;
        _configurationRepository = configurationRepository;
        _gameVersionRepository = gameVersionRepository;
        _consulClient = consulClient;
        _consulGameService = consulGameService;
    }

    public InitialDataResponseModel GetInitialData()
    {
        return new InitialDataResponseModel
        {
            PrizeGroups = _configurationHolder.PrizeGroups,
            Prices = _configurationHolder.Prices,
        };
    }

    public List<GameRegisterResponseModel> GetGames()
    {
        var activeGames = _gameVersionRepository.Query()
                                                .Include(x => x.Configurations)
                                                .Where(x => x.IsActive)
                                                .ToList();

        if (activeGames == null)
        {
            throw new InvalidOperationException("No active game version found for the provided SegmentIds.");
        }

        var responseList = activeGames.Select(gameVersion => new GameRegisterResponseModel
        {
            GameVersionId = gameVersion.Id,
            GameVersionName = gameVersion.Name,
            GameVersionIsActive = gameVersion.IsActive,
            Address = "wheel",
            SegmentIds = gameVersion.SegmentIds.ToList(),
            ActivationTime = DateTime.UtcNow,
            Configurations = gameVersion.Configurations
                                         .Select(config => new ConfigurationResponseModel
                                         {
                                             Id = config.Id,
                                             Name = config.Name,
                                             IsDefault = config.IsDefault,
                                             IsActive = config.IsActive
                                         })
                                         .ToList()
        }).ToList();


        return responseList;
    }

    public async Task UpdateMetadataAsync()
    {
        await _consulGameService.UpdateMetadataAsync(
            getDataFunc: GetGames,
            serviceId: "wheelapi",
            serviceName: "wheelapi",
            port: 8080,
            tags: new[] { "Game", "Back" }
        );
    }

    public async Task<PlayResponseModel> PlayJackpotAsync(PlayRequestModel command)
    {
        return await PlayAsync<JackpotPrize>(command);
    }

    public async Task<PlayResponseModel> PlayWheelAsync(PlayRequestModel command)
    {
        return await PlayAsync<WheelPrize>(command);
    }

    private async Task<PlayResponseModel> PlayAsync<TPrize>(PlayRequestModel command)
        where TPrize : BasePrize
    {
        await _hubService.BetTransactionAsync(command.GameId);

        var ttt = _authService.GetCurrentPlayerSegmentIds().ToList()[0];

        var prize = GeneratorHolder.GetPrize<TPrize>(command.GameVersionId, _authService.GetCurrentPlayerSegmentIds().ToList()[0]);

        if (prize == null)
        {
            throw new ArgumentNullException(nameof(prize));
        }

        if (prize.Value > 0)
        {
            await _hubService.WinTransactionAsync(command.GameId);
        }

        return new PlayResponseModel
        {
            PrizeResults = new List<BasePrize> { prize },
            Multiplier = 0,
        };
    }
}