using AggregationService.Application.Models.AggregationConfigurations;

namespace AggregationService.Application.Services.Abstract;

public interface IAggregationConfigurationService
{
    Task AddAggregationWithConfigurationsAsync(CreateAggregationConfigurationModel model);
    Task UpdateAggregationAsync(UpdateAggregationConfigurationModel model);
}