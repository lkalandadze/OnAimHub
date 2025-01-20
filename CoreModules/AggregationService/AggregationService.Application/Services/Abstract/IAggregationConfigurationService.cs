using AggregationService.Application.Models.AggregationConfigurations;
using AggregationService.Domain.Entities;
using Shared.IntegrationEvents.IntegrationEvents.Aggregation;

namespace AggregationService.Application.Services.Abstract;

public interface IAggregationConfigurationService
{
    Task AddAggregationWithConfigurationsAsync(CreateAggregationConfigurationModel model);
    Task UpdateAggregationAsync(UpdateAggregationConfigurationModel model);
    Task TriggerRequestAsync(AggregationTriggerEvent @event, AggregationConfiguration config);
    Task Test(AggregationTriggerEvent test, CancellationToken cancellationToken);
}