using LevelService.Application.Models.Configurations;

namespace LevelService.Application.Features.ConfigurationFeatures.Queries.Get;

public class GetConfigurationsQueryResponse
{
    public IEnumerable<ConfigurationModel>? Configurations { get; set; }
}
