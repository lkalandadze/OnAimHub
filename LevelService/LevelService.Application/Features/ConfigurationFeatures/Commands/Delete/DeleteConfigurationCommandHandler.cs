using LevelService.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LevelService.Application.Features.ConfigurationFeatures.Commands.Delete;

public class DeleteConfigurationCommandHandler : IRequestHandler<DeleteConfigurationCommand>
{
    private readonly IStageRepository _stageRepository;
    public DeleteConfigurationCommandHandler(IStageRepository stageRepository)
    {
        _stageRepository = stageRepository;
    }

    public async Task Handle(DeleteConfigurationCommand command, CancellationToken cancellationToken)
    {
        var stage = await _stageRepository.Query().Include(x => x.Configurations).FirstOrDefaultAsync(x => x.Id.Equals(command.StageId), cancellationToken);

        if (stage == default)
            throw new Exception("Stage not found");

        var configuration = stage.Configurations.FirstOrDefault(c => c.Id == command.ConfigurationId);

        if (configuration == null)
            throw new Exception("Configuration not found");

        configuration.Delete();

        await _stageRepository.SaveChangesAsync(cancellationToken);
    }
}
