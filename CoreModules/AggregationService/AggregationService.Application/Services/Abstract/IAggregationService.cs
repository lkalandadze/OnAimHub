using AggregationService.Application.Models.Aggregations;

namespace AggregationService.Application.Services.Abstract;

public interface IAggregationService
{
    Task AddAggregationWithConfigurationsAsync(CreateAggregationModel request);
    Task UpdateAggregationAsync(UpdateAggregationModel request);
}