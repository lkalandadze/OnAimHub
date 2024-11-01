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

    public PlayerService(IPlayerRepository playerRepository, IConfigurationRepository configurationRepository, IStageRepository stageRepository)
    {
        _playerRepository = playerRepository;
        _configurationRepository = configurationRepository;
        _stageRepository = stageRepository;
    }

    public async Task GrantExperienceAndRewardsAsync(int playerId, string currencyId, int amount)
    {
        var player = await _playerRepository.Query()
                                            .Include(x => x.PlayerRewards)
                                            .FirstOrDefaultAsync(x => x.Id == playerId);

        if (player == default) throw new Exception("Player not found");

        var configuration = await _configurationRepository.Query()
                                                          .FirstOrDefaultAsync(c => c.CurrencyId == currencyId);

        if (configuration == default) throw new Exception("Configuration not found for the specified CurrencyId");

        var totalExperienceToAdd = configuration.ExperienceToGrant * amount;

        var activeStage = await _stageRepository.Query()
                                                .Include(x => x.Levels)
                                                .ThenInclude(x => x.LevelPrizes)
                                                .FirstOrDefaultAsync(stage => stage.Status == StageStatus.InProgress);

        if (activeStage == default) throw new Exception("Active stage not found");

        player.AddExperience(totalExperienceToAdd, activeStage);

        await _playerRepository.SaveChangesAsync();
    }
}