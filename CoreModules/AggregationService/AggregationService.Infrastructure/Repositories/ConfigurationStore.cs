using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace AggregationService.Infrastructure.Repositories;

public class ConfigurationStore : IConfigurationStore
{
    private readonly IServiceProvider _serviceProvider;

    // Thread-safe in-memory store
    private readonly ConcurrentDictionary<string, ConcurrentBag<AggregationConfiguration>> _memoryStore = new();

    public ConfigurationStore(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private IAggregationConfigurationRepository GetRepository()
    {
        try
        {
            return _serviceProvider.GetRequiredService<IAggregationConfigurationRepository>();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task AddConfigurationsAsync(List<AggregationConfiguration> configs)
    {
        var repository = GetRepository();
        await repository.AddConfigurationsAsync(configs);

        foreach (var config in configs)
        {
            AddConfigInMemory(config);
        }
    }

    private void AddConfigInMemory(AggregationConfiguration config)
    {
        var configList = _memoryStore.GetOrAdd(config.EventProducer, _ => new ConcurrentBag<AggregationConfiguration>());
        configList.Add(config);
    }

    public IEnumerable<AggregationConfiguration> GetAllConfigurations()
    {
        return _memoryStore.Values.SelectMany(configs => configs);
    }

    public IEnumerable<AggregationConfiguration> GetConfigurationsByProducer(string eventProducer)
    {
        if (_memoryStore.TryGetValue(eventProducer, out var configs))
        {
            return configs;
        }

        return Enumerable.Empty<AggregationConfiguration>();
    }

    public async Task ReloadConfigurationsAsync()
    {
        var repository = GetRepository();
        var configs = await repository.GetAllConfigurationsAsync();

        _memoryStore.Clear();
        foreach (var config in configs)
        {
            AddConfigInMemory(config);
        }
    }
}