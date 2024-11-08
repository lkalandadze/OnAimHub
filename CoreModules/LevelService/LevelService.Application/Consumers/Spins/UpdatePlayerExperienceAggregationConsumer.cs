using LevelService.Domain.Abstractions.Repository;
using LevelService.Domain.Enum;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.IntegrationEvents.IntegrationEvents.Player;

namespace LevelService.Application.Consumers.Spins;

public class UpdatePlayerExperienceAggregationConsumer : IConsumer<UpdatePlayerExperienceEvent>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IStageRepository _stageRepository;
    public UpdatePlayerExperienceAggregationConsumer(IPlayerRepository playerRepository, IConfigurationRepository configurationRepository, IStageRepository stageRepository)
    {
        _playerRepository = playerRepository;
        _configurationRepository = configurationRepository;
        _stageRepository = stageRepository;
    }

    public async Task Consume(ConsumeContext<UpdatePlayerExperienceEvent> context)
    {
        var data = context.Message;

        var player = await _playerRepository.Query().Include(x => x.PlayerRewards).FirstOrDefaultAsync(x => x.Id == data.PlayerId);

        if (player == default)
            throw new Exception("Player not found");

        var stage = await _stageRepository.Query().Include(x => x.Levels).ThenInclude(x => x.LevelPrizes).FirstOrDefaultAsync(x => x.Status == StageStatus.InProgress);

        if (stage == default)
            throw new Exception("Stage not found");

        var configuration = await _configurationRepository.Query().FirstOrDefaultAsync(x => x.CurrencyId == data.CurrencyId && !x.IsDeleted);

        if (configuration == default)
            throw new Exception("Configuration not found");

        var experienceToAdd = data.Amount * configuration.ExperienceToGrant;

        player.AddExperience(experienceToAdd, stage);

        _playerRepository.Update(player);
        await _playerRepository.SaveChangesAsync();
    }
}
