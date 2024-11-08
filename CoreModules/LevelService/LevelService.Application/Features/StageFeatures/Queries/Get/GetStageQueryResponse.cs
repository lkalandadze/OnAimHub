using LevelService.Application.Models.Stages;

namespace LevelService.Application.Features.StageFeatures.Queries.Get;

public class GetStageQueryResponse
{
    public IEnumerable<StageModel>? Stages { get; set; }
}
