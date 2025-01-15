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
        Console.WriteLine($"Adding configuration to memory store: {System.Text.Json.JsonSerializer.Serialize(config)}");
        var configList = _memoryStore.GetOrAdd(config.EventProducer.ToLower(), _ => new ConcurrentBag<AggregationConfiguration>());
        configList.Add(config);
    }

    public async Task ReloadConfigurationsAsync()
    {
        var repository = GetRepository();
        var configs = await repository.GetAllConfigurationsAsync();

        Console.WriteLine($"Reloading configurations from repository: {System.Text.Json.JsonSerializer.Serialize(configs)}");

        _memoryStore.Clear();
        foreach (var config in configs)
        {
            AddConfigInMemory(config);
        }
    }

    public IEnumerable<AggregationConfiguration> GetAllConfigurations()
    {
        return _memoryStore.Values.SelectMany(configs => configs);
    }

    public IEnumerable<AggregationConfiguration> GetConfigurationsByProducer(string eventProducer)
    {
        if (_memoryStore.TryGetValue(eventProducer.ToLower(), out var configs))
        {
            return configs;
        }

        return Enumerable.Empty<AggregationConfiguration>();
    }
}