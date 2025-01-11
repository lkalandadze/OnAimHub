using AggregationService.Domain.Entities;

namespace AggregationService.Domain.Abstractions.Repository;

public interface IAggregationConfigurationRepository : IBaseRepository<AggregationConfiguration> 
{
    Task AddConfigurationsAsync(List<AggregationConfiguration> configs);
    Task<List<AggregationConfiguration>> GetAllConfigurationsAsync();
}
