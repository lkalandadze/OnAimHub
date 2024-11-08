using LevelService.Domain.Abstractions.Repository;
using LevelService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LevelService.Application.Features.LevelFeatures.Commands.Create;

public class CreateLevelsCommandHandler : IRequestHandler<CreateLevelsCommand>
{
    private readonly IStageRepository _stageRepository;
    public CreateLevelsCommandHandler(IStageRepository stageRepository)
    {
        _stageRepository = stageRepository;
    }

    public async Task Handle(CreateLevelsCommand request, CancellationToken cancellationToken)
    {
        var stage = await _stageRepository.Query().Include(x => x.Levels).ThenInclude(x => x.LevelPrizes).FirstOrDefaultAsync(x => x.Id.Equals(request.StageId), cancellationToken);

        if (stage == default)
            throw new Exception("Stage not found");

        foreach (var levelModel in request.Levels)
        {
            var level = new Level(levelModel.Number, levelModel.ExperienceToArchive)
            {
                LevelPrizes = levelModel.Prizes.Select(prizeModel => new LevelPrize(
                    prizeModel.Amount,
                    prizeModel.PrizeTypeId,
                    prizeModel.PrizeDeliveryType)).ToList()
            };
            stage.Levels.Add(level);
        }

        await _stageRepository.SaveChangesAsync(cancellationToken);
    }
}