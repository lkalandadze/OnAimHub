using LevelService.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LevelService.Application.Features.ConfigurationFeatures.Commands.Update;

public class UpdateConfigurationCommandHandler : IRequestHandler<UpdateConfigurationCommand>
{
    private readonly IStageRepository _stageRepository;
    public UpdateConfigurationCommandHandler(IStageRepository stageRepository)
    {
        _stageRepository = stageRepository;
    }

    public async Task Handle(UpdateConfigurationCommand request, CancellationToken cancellationToken)
    {
        var stage = await _stageRepository.Query().Include(x => x.Configurations).FirstOrDefaultAsync(x => x.Id.Equals(request.StageId));

        if (stage == default)
            throw new Exception("Stage not found");

        var configuration = stage.Configurations.FirstOrDefault(c => c.Id == request.ConfigurationId);

        if (configuration == null)
            throw new Exception("Configuration not found");

        bool currencyIdExists = stage.Configurations
            .Any(c => c.CurrencyId == request.CurrencyId && c.Id != request.ConfigurationId && !c.IsDeleted);

        if (currencyIdExists)
            throw new Exception($"A configuration with CurrencyId '{request.CurrencyId}' already exists for StageId '{request.StageId}'.");

        stage.UpdateConfiguration(request.ConfigurationId, request.CurrencyId, request.ExperienceToGrant);

        await _stageRepository.SaveChangesAsync(cancellationToken);
    }
}