using Microsoft.AspNetCore.Http;
using Shared.Application.Holders;
using Shared.Application.Services.Abstract;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Repository;
using Wheel.Application.Models;
using Wheel.Domain.Entities;
using Wheel.Infrastructure.Services.Abstract;

namespace Wheel.Infrastructure.Services.Concrete;

public class GameService : IGameService
{
    private readonly GeneratorHolder _generatorHolder;
    private readonly ConfigurationHolder _configurationHolder;
    private readonly IAuthService _authService;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IPrizeGroupRepository<WheelPrizeGroup> _prizeGroupRepository;
    private readonly IGameVersionRepository _gameVersionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GameService(
        GeneratorHolder generatorHolder,
        ConfigurationHolder configurationHolder,
        IAuthService authService,
        IConfigurationRepository configurationRepository,
        IPrizeGroupRepository<WheelPrizeGroup> prizeGroupRepository,
        IGameVersionRepository gameVersionRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _generatorHolder = generatorHolder;
        _configurationHolder = configurationHolder;
        _authService = authService;
        _configurationRepository = configurationRepository;
        _prizeGroupRepository = prizeGroupRepository;
        _gameVersionRepository = gameVersionRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public InitialDataResponseModel GetInitialData()
    {
        return new InitialDataResponseModel
        {
            PrizeGroups = _configurationHolder.PrizeGroups,
            Prices = _configurationHolder.Prices,
        };
    }

    public GameVersionResponseModel GetGame()
    {
        var gameVersion = _gameVersionRepository.Query()
        .FirstOrDefault();

        if (gameVersion == null)
        {
            throw new InvalidOperationException("No active game version found for the provided SegmentIds.");
        }

        return new GameVersionResponseModel
        {
            Id = gameVersion.Id,
            Name = gameVersion.Name,
            
            IsActive = gameVersion.IsActive,
            SegmentIds = gameVersion.SegmentIds.ToList(),
            ActivationTime = DateTime.UtcNow,
        };
    }

    public PlayResultModel Play(PlayRequestModel command)
    {
        var prize = GeneratorHolder.GetPrize<JackpotPrize>(command.GameVersionId, _authService.GetCurrentPlayerSegmentId());

        return new PlayResultModel
        {
            PrizeResults = new List<BasePrize> { prize },
            Multiplier = 0,
        };
    }
}