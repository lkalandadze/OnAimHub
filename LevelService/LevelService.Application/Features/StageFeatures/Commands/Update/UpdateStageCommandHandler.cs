using LevelService.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LevelService.Application.Features.StageFeatures.Commands.Update;

public class UpdateStageCommandHandler : IRequestHandler<UpdateStageCommand>
{
    private readonly IStageRepository _stageRepository;
    public UpdateStageCommandHandler(IStageRepository stageRepository)
    {
        _stageRepository = stageRepository;
    }

    public async Task Handle(UpdateStageCommand request, CancellationToken cancellationToken)
    {
        var stage = await _stageRepository.Query().FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

        if (stage == default)
            throw new Exception("Stage not found");

        stage.Update(request.Name, request.Description, request.DateFrom, request.DateTo, request.IsExpirable);

        await _stageRepository.SaveChangesAsync(cancellationToken);
    }
}