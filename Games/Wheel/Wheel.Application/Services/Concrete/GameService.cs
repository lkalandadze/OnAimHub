﻿using GameLib.Application.Holders;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using Wheel.Application.Models.Game;
using Wheel.Application.Models.Player;
using Wheel.Application.Services.Abstract;
using Wheel.Domain.Entities;

namespace Wheel.Application.Services.Concrete;

public class GameService : IGameService
{
    private readonly ConfigurationHolder _configurationHolder;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IAuthService _authService;
    private readonly IHubService _hubService;
    private readonly IConsulGameService _consulGameService;

    public GameService(
        ConfigurationHolder configurationHolder,
        IConfigurationRepository segmentRepository,
        IAuthService authService,
        IHubService hubService,
        IConsulGameService consulGameService)
    {
        _configurationHolder = configurationHolder;
        _configurationRepository = segmentRepository;
        _authService = authService;
        _hubService = hubService;
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

    public GameResponseModel GetGame()
    {
        var segments = _configurationRepository.Query();

        return new GameResponseModel
        {
            Name = "Wheel",
            SegmentIds = segments == null ? default : segments.Select(x => x.Name).ToList(),
            ActivationTime = DateTime.UtcNow,
        };
    }

    public async Task UpdateMetadataAsync()
    {
        await _consulGameService.UpdateMetadataAsync(
            getDataFunc: GetGame,
            serviceId: "WheelApi",
            serviceName: "WheelApi",
            port: 8080,
            tags: ["Game", "Back"]
        );
    }

    public async Task<PlayResponseModel> PlayJackpotAsync(PlayRequestModel model)
    {
        return await PlayAsync<JackpotPrize>(model);
    }

    public async Task<PlayResponseModel> PlayWheelAsync(PlayRequestModel model)
    {
        return await PlayAsync<WheelPrize>(model);
    }

    private async Task<PlayResponseModel> PlayAsync<TPrize>(PlayRequestModel model)
        where TPrize : BasePrize
    {
        await _hubService.BetTransactionAsync(model.GameId, model.CurrencyId, model.Amount);

        //TODO: prioritetis minicheba segmentistvis
        var prize = GeneratorHolder.GetPrize<TPrize>(_authService.GetCurrentPlayerSegmentIds().ToList()[0]);

        if (prize == null)
        {
            throw new ArgumentNullException(nameof(prize));
        }

        if (prize.Value > 0)
        {
            await _hubService.WinTransactionAsync(model.GameId, model.CurrencyId, model.Amount);
        }

        return new PlayResponseModel
        {
            PrizeResults = [prize],
            Multiplier = 0,
        };
    }
}