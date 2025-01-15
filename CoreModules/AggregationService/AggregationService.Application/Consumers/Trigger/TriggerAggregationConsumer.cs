using AggregationService.Application.Services.Abstract;
using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Entities;
using AggregationService.Domain.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Shared.IntegrationEvents.IntegrationEvents.Aggregation;

namespace AggregationService.Application.Consumers.Trigger;

public sealed class TriggerAggregationConsumer : IConsumer<TriggerAggregationEvent>
{
    private readonly IAggregationConfigurationService _aggregationConfigurationService;
    private readonly IAggregationConfigurationRepository _aggregationConfigurationRepository;
    private readonly IConfigurationStore _configurationStore;

    public TriggerAggregationConsumer(
        IAggregationConfigurationService aggregationConfigurationService,
        IAggregationConfigurationRepository aggregationConfigurationRepository,
        IConfigurationStore configurationStore)
    {
        _aggregationConfigurationService = aggregationConfigurationService;
        _aggregationConfigurationRepository = aggregationConfigurationRepository;
        _configurationStore = configurationStore;
    }

    public async Task Consume(ConsumeContext<TriggerAggregationEvent> context)
    {
        var cancellationToken = context.CancellationToken;
        var request = context.Message;

        Console.WriteLine($"Received event: {System.Text.Json.JsonSerializer.Serialize(request)}");

        var filteredConfigurations = _configurationStore.GetAllConfigurations().Filter(request);

        if (!filteredConfigurations.Any())
        {
            throw new InvalidOperationException($"No matching configurations found. Request: {System.Text.Json.JsonSerializer.Serialize(request)}");
        }

        var promotionIds = filteredConfigurations
            .Select(config => config.PromotionId)
            .Distinct()
            .ToList();

        Console.WriteLine($"Promotion IDs: {string.Join(", ", promotionIds)}");

        var filter = Builders<AggregationConfiguration>.Filter.In(config => config.PromotionId, promotionIds);
        var configurations = await _aggregationConfigurationRepository
            .GetCollection()
            .Find(filter)
            .ToListAsync(cancellationToken);

        if (!configurations.Any())
        {
            throw new InvalidOperationException($"No configurations found for PromotionIds: {string.Join(", ", promotionIds)}");
        }

        foreach (var config in configurations)
        {
            await _aggregationConfigurationService.TriggerRequestAsync(request, config);
        }

        Console.WriteLine("Trigger aggregation successfully processed.");
    }
}