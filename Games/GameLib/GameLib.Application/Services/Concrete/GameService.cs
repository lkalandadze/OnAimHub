using GameLib.Application.Configurations;
using GameLib.Application.Holders;
using GameLib.Application.Models.Game;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions.Repository;
using Microsoft.Extensions.Options;

namespace GameLib.Application.Services.Concrete;

public class GameService : IGameService
{
    private readonly IGameConfigurationRepository _configurationRepository;
    private readonly IConsulGameService _consulGameService;
    private readonly ConfigurationHolder _configurationHolder;
    private readonly GameSettings _gameSettings;
    private readonly GameInfoConfiguration _gameInfoConfig;

    public GameService(
        IGameConfigurationRepository configurationRepository, 
        IConsulGameService consulGameService, 
        ConfigurationHolder configurationHolder, 
        GameSettings gameSettings,
        IOptions<GameInfoConfiguration> gameInfoConfig)
    {
        _configurationRepository = configurationRepository;
        _consulGameService = consulGameService;
        _configurationHolder = configurationHolder;
        _gameSettings = gameSettings;
        _gameInfoConfig = gameInfoConfig.Value;
    }

    public InitialDataResponseModel GetInitialData()
    {
        return new InitialDataResponseModel
        {
            PrizeGroups = _configurationHolder.PrizeGroups,
            Prices = _configurationHolder.Prices,
        };
    }

    public GameResponseModel GetGame()
    {
        var segments = _configurationRepository.Query();

        return new GameResponseModel
        {
            Name = _gameInfoConfig.Name,
            ActivationTime = DateTime.UtcNow,
        };
    }

    public async Task UpdateMetadataAsync()
    {
        await _consulGameService.UpdateMetadataAsync(
            getDataFunc: GetGame,
            serviceId: _gameInfoConfig.ApiName,
            serviceName: _gameInfoConfig.ApiName,
            port: 8080,
            tags: ["Game", "Back"]
        );
    }

    public async Task<GameShortInfoResponseModel> GetGameShortInfo()
    {
        var configurations = await _configurationRepository.QueryAsync();

        return new GameShortInfoResponseModel
        {
            Status = _gameSettings.IsActive.Value,
            Description = _gameSettings.Description.Value,
            ConfigurationCount = configurations.Count(),
        };
    }

    public bool GameStatus()
    {
        return _gameSettings.IsActive.Value;
    }

    public void ActivateGame()
    {
        _gameSettings.SetValue(nameof(_gameSettings.IsActive), true);
    }

    public void DeactivateGame()
    {
        _gameSettings.SetValue(nameof(_gameSettings.IsActive), false);
    }
}