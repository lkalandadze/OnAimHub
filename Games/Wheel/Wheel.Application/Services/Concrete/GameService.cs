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

    public GameService(
        GeneratorHolder generatorHolder,
        ConfigurationHolder configurationHolder,
        IAuthService authService,
        IHubService hubService,
        IConfigurationRepository configurationRepository,
        IGameVersionRepository gameVersionRepository,
        IConsulClient consulClient)
    {
        _generatorHolder = generatorHolder;
        _configurationHolder = configurationHolder;
        _authService = authService;
        _hubService = hubService;
        _configurationRepository = configurationRepository;
        _gameVersionRepository = gameVersionRepository;
        _consulClient = consulClient;
    }

    public InitialDataResponseModel GetInitialData()
    {
        return new InitialDataResponseModel
        {
            PrizeGroups = _configurationHolder.PrizeGroups,
            Prices = _configurationHolder.Prices,
        };
    }

    public List<GameRegisterResponseModel> GetGame()
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
        var activeGameModel = GetGame();

        var serializedGameData = JsonSerializer.Serialize(activeGameModel);

        var serviceId = "wheelapi";

        var registration = new AgentServiceRegistration
        {
            ID = serviceId,
            Name = "wheelapi",
            Address = "wheelapi",
            Port = 8080,
            Tags = new[] { "Game", "Back" },
            Meta = new Dictionary<string, string>
            {
                { "GameData", serializedGameData }
            }
        };

        await _consulClient.Agent.ServiceRegister(registration);
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
        await _hubService.BetTransactionAsync(command.GameVersionId);

        var ttt = _authService.GetCurrentPlayerSegmentIds().ToList()[0];

        var prize = GeneratorHolder.GetPrize<TPrize>(command.GameVersionId, _authService.GetCurrentPlayerSegmentIds().ToList()[0]);

        if (prize == null)
        {
            throw new ArgumentNullException(nameof(prize));
        }

        if (prize.Value > 0)
        {
            await _hubService.WinTransactionAsync(command.GameVersionId);
        }

        return new PlayResponseModel
        {
            PrizeResults = new List<BasePrize> { prize },
            Multiplier = 0,
        };
    }
}