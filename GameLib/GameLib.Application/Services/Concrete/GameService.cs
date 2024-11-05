﻿using GameLib.Application.Holders;
using GameLib.Application.Models.Game;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions.Repository;

namespace GameLib.Application.Services.Concrete;

public class GameService : IGameService
{
    private readonly ISegmentRepository _segmentRepository;
    private readonly IGameConfigurationRepository _configurationRepository;
    private readonly ConfigurationHolder _configurationHolder;
    private readonly GameSettings _gameSettings;

    public GameService(ISegmentRepository segmentRepository, IGameConfigurationRepository configurationRepository, ConfigurationHolder configurationHolder, GameSettings gameSettings)
    {
        _segmentRepository = segmentRepository;
        _configurationRepository = configurationRepository;
        _configurationHolder = configurationHolder;
        _gameSettings = gameSettings;
    }

    public GetInitialDataResponseModel GetInitialData()
    {
        return new GetInitialDataResponseModel
        {
            PrizeGroups = _configurationHolder.PrizeGroups,
            Prices = _configurationHolder.Prices,
        };
    }

    public async Task<GetGameShortInfoModel> GetGameShortInfo()
    {
        var configurations = await _configurationRepository.QueryAsync();
        var segments = await _segmentRepository.QueryAsync();

        return new GetGameShortInfoModel
        {
            Status = _gameSettings.IsActive.Value,
            Description = _gameSettings.Description.Value,
            ConfigurationCount = configurations.Count(),
            Segments = segments.Select(s => s.Id),
        };
    }

    public bool GameStatus()
    {
        return _gameSettings.IsActive.Value;
    }

    public void ActivateGame()
    {
        _gameSettings.SetValue(_gameSettings.IsActive, nameof(_gameSettings.IsActive), true);
    }

    public void DeactivateGame()
    {
        _gameSettings.SetValue(_gameSettings.IsActive, nameof(_gameSettings.IsActive), false);
    }
}