using AggregationService.Application.Models.Request;
using AggregationService.Application.Models.Response.AggregationConfigurations;

namespace AggregationService.Application.Services.Abstract;

public interface IAggregationConfigurationService
{
    Task AddAggregationWithConfigurationsAsync(CreateAggregationConfigurationModel model);
    Task UpdateAggregationAsync(UpdateAggregationConfigurationModel model);
    Task<List<string>> ProcessPlayRequestAsync(int playerId, string coinIn, decimal amount, int promotionId);
}