using AggregationService.Domain.Entities;
using MongoDB.Driver;

namespace AggregationService.Domain.Abstractions.Repository;

public interface IAggregationConfigurationRepository
{
    IMongoCollection<AggregationConfiguration> GetCollection();
    Task AddConfigurationsAsync(List<AggregationConfiguration> configs);
    Task<List<AggregationConfiguration>> GetAllConfigurationsAsync();
    Task UpdateAsync(AggregationConfiguration aggregation, FilterDefinition<AggregationConfiguration> filter);
}
