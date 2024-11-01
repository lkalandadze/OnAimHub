using LevelService.Application.Services.Abstract;
using LevelService.Domain.Abstractions.Repository;
using LevelService.Domain.Entities;
using LevelService.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace LevelService.Application.Services.Concrete;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IStageRepository _stageRepository;

    // Cache fields for configuration and active stage
    private Configuration _cachedConfiguration;
    private DateTime _configurationLastUpdated;
    private Stage _cachedActiveStage;
    private DateTime _activeStageLastUpdated;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

    public PlayerService(IPlayerRepository playerRepository, IConfigurationRepository configurationRepository, IStageRepository stageRepository)
    {
        _playerRepository = playerRepository;
        _configurationRepository = configurationRepository;
        _stageRepository = stageRepository;
    }

    // Helper method to get or refresh the cached configuration
    private async Task<Configuration> GetConfigurationAsync(string currencyId)
    {
        if (_cachedConfiguration == null || DateTime.UtcNow - _configurationLastUpdated > _cacheDuration)
        {
            _cachedConfiguration = await _configurationRepository.Query()
                                                                 .FirstOrDefaultAsync(c => c.CurrencyId == currencyId);
            _configurationLastUpdated = DateTime.UtcNow;

            if (_cachedConfiguration == null)
                throw new Exception("Configuration not found for the specified CurrencyId");
        }
        return _cachedConfiguration;
    }

    // Helper method to get or refresh the cached active stage
    private async Task<Stage> GetActiveStageAsync()
    {
        if (_cachedActiveStage == null || DateTime.UtcNow - _activeStageLastUpdated > _cacheDuration)
        {
            _cachedActiveStage = await _stageRepository.Query()
                                                       .Include(x => x.Levels)
                                                       .ThenInclude(x => x.LevelPrizes)
                                                       .FirstOrDefaultAsync(stage => stage.Status == StageStatus.InProgress);
            _activeStageLastUpdated = DateTime.UtcNow;

            if (_cachedActiveStage == null)
                throw new Exception("Active stage not found");
        }
        return _cachedActiveStage;
    }

    public async Task GrantExperienceAndRewardsAsync(int playerId, string currencyId, int amount)
    {
        var player = await _playerRepository.Query()
                                            .Include(x => x.PlayerRewards)
                                            .FirstOrDefaultAsync(x => x.Id == playerId);

        if (player == default) throw new Exception("Player not found");

        var configuration = await GetConfigurationAsync(currencyId);
        var activeStage = await GetActiveStageAsync();

        var totalExperienceToAdd = configuration.ExperienceToGrant * amount;

        player.AddExperience(totalExperienceToAdd, activeStage);

        await _playerRepository.SaveChangesAsync();
    }
}