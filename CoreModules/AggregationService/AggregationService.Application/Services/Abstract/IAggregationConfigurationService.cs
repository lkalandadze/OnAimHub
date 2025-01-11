using AggregationService.Application.Models.Response.AggregationConfigurations;
using AggregationService.Domain.Entities;
using Shared.IntegrationEvents.IntegrationEvents.Aggregation;

namespace AggregationService.Application.Services.Abstract;

public interface IAggregationConfigurationService
{
    Task AddAggregationWithConfigurationsAsync(CreateAggregationConfigurationModel model);
    Task UpdateAggregationAsync(UpdateAggregationConfigurationModel model);
    Task TriggerRequestAsync(TriggerAggregationEvent @event, AggregationConfiguration config);
}