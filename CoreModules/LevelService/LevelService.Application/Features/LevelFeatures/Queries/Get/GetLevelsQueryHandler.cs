using LevelService.Application.Features.LevelFeatures.Helper;
using LevelService.Application.Features.StageFeatures.Queries.Get;
using LevelService.Application.Models.Levels;
using LevelService.Application.Models.Stages;
using LevelService.Domain.Abstractions.Repository;
using LevelService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LevelService.Application.Features.LevelFeatures.Queries.Get;

public class GetLevelsQueryHandler : IRequestHandler<GetLevelsQuery, GetLevelsQueryResponse>
{
    private readonly IStageRepository _stageRepository;
    public GetLevelsQueryHandler(IStageRepository stageRepository)
    {
        _stageRepository = stageRepository;
    }

    public async Task<GetLevelsQueryResponse> Handle(GetLevelsQuery request, CancellationToken cancellationToken)
    {
        var stage = await _stageRepository.Query()
                                          .Include(x => x.Levels)
                                          .ThenInclude(x => x.LevelPrizes)
                                          .FirstOrDefaultAsync(x => x.Id.Equals(request.StageId), cancellationToken);

        if (stage == default)
            throw new Exception("Stage not found");

        var levels = stage.Levels.Select(LevelModelExtensions.MapFrom);

        return new GetLevelsQueryResponse
        {
            Levels = levels
        };
    }
}