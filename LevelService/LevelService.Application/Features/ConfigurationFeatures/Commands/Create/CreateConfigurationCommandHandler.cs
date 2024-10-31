using LevelService.Application.Features.LevelFeatures.Commands.Create;
using LevelService.Domain.Abstractions.Repository;
using LevelService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LevelService.Application.Features.ConfigurationFeatures.Commands.Create;

public class CreateConfigurationCommandHandler : IRequestHandler<CreateConfigurationCommand>
{
    private readonly IStageRepository _stageRepository;
    public CreateConfigurationCommandHandler(IStageRepository stageRepository)
    {
        _stageRepository = stageRepository;
    }

    public async Task Handle(CreateConfigurationCommand request, CancellationToken cancellationToken)
    {
        var stage = await _stageRepository.Query().Include(x => x.Configurations).FirstOrDefaultAsync(x => x.Id.Equals(request.StageId), cancellationToken);

        if (stage == default)
            throw new Exception("Stage not found");

        foreach (var configurationModel in request.Configurations)
        {
            bool exists = stage.Configurations.Any(c => c.CurrencyId == configurationModel.CurrencyId && !c.IsDeleted);
            if (exists)

                throw new Exception($"Configuration with CurrencyId '{configurationModel.CurrencyId}' already exists for StageId '{request.StageId}'.");

            var level = new Configuration(configurationModel.CurrencyId, configurationModel.ExperienceToGrant);

            stage.Configurations.Add(level);
        }

        await _stageRepository.SaveChangesAsync(cancellationToken);
    }
}