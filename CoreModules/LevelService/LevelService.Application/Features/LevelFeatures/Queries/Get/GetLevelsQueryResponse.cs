using LevelService.Application.Models.Levels;

namespace LevelService.Application.Features.LevelFeatures.Queries.Get;

public class GetLevelsQueryResponse
{
    public IEnumerable<LevelModel>? Levels { get; set; }
}