using LevelService.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LevelService.Application.Features.StageFeatures.Commands.Delete;

public class DeleteStageCommandHandler : IRequestHandler<DeleteStageCommand>
{
    private readonly IStageRepository _stageRepository;
    public DeleteStageCommandHandler(IStageRepository stageRepository)
    {
        _stageRepository = stageRepository;
    }

    public async Task Handle(DeleteStageCommand request, CancellationToken cancellationToken)
    {
        var stage = await _stageRepository.Query().FirstOrDefaultAsync(x => x.Id.Equals(request.StageId), cancellationToken);

        if (stage == default)
            throw new Exception("Stage not found");

        stage.Delete();
    }
}