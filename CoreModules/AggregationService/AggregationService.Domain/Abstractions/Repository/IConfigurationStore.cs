using AggregationService.Domain.Entities;

namespace AggregationService.Domain.Abstractions.Repository;

public interface IConfigurationStore
{
    Task AddConfigurationsAsync(List<AggregationConfiguration> configs);
    IEnumerable<AggregationConfiguration> GetAllConfigurations();
    Task ReloadConfigurationsAsync();
    IEnumerable<AggregationConfiguration> GetConfigurationsByProducer(string eventProvider);

}