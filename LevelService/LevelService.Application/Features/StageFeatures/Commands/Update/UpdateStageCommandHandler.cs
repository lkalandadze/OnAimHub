using LevelService.Domain.Abstractions.Repository;
using MediatR;

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
        var stage = _stageRepository.Query().FirstOrDefault(x => x.Id.Equals(request.Id));

        if (stage == default)
            throw new Exception("Stage not found");

        stage.Update(request.Name, request.Description, request.DateFrom, request.DateTo, request.IsExpirable);
    }
}