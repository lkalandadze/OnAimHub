using GameLib.Application.Holders;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using Wheel.Application.Models.Game;
using Wheel.Application.Models.Player;
using Wheel.Application.Services.Abstract;
using Wheel.Domain.Abstractions.Repository;
using Wheel.Domain.Entities;

namespace Wheel.Application.Services.Concrete;

public class GameService : IGameService
{
    private readonly ConfigurationHolder _configurationHolder;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IAuthService _authService;
    private readonly IHubService _hubService;
    private readonly IConsulGameService _consulGameService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoundRepository _roundRepository;
    private readonly IWheelPrizeRepository _wheelPrizeRepository;
    private readonly ISegmentRepository _segmentRepository;
    public GameService(
        ConfigurationHolder configurationHolder,
        IConfigurationRepository configurationRepository,
        IAuthService authService,
        IHubService hubService,
        IConsulGameService consulGameService,
        IUnitOfWork unitOfWork,
        IRoundRepository roundRepository,
        IWheelPrizeRepository wheelPrizeRepository,
        ISegmentRepository segmentRepository)
    {
        _configurationHolder = configurationHolder;
        _configurationRepository = configurationRepository;
        _authService = authService;
        _hubService = hubService;
        _consulGameService = consulGameService;
        _unitOfWork = unitOfWork;
        _roundRepository = roundRepository;
        _wheelPrizeRepository = wheelPrizeRepository;
        _segmentRepository = segmentRepository;
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
        return new GameResponseModel
        {
            Name = "Wheel",
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