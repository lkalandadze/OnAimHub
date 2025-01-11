using AggregationService.Application.Services.Abstract;
using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Extensions;
using Consul;
using MassTransit;
using Microsoft.EntityFrameworkCore;
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

        var filteredConfigurations = _configurationStore.GetAllConfigurations().Filter(request);

        var promotionIds = filteredConfigurations.Select(config => config.PromotionId).Distinct().ToList();

        if (!promotionIds.Any())
            throw new InvalidOperationException($"No configurations found for the given event.");

        var configurations = await _aggregationConfigurationRepository.Query()
            .Where(config => promotionIds.Contains(config.PromotionId))
            .ToListAsync(cancellationToken);

        if (!configurations.Any())
            throw new InvalidOperationException($"No configurations found for PromotionIds: {string.Join(", ", promotionIds)}");

        foreach (var config in configurations)
            await _aggregationConfigurationService.TriggerRequestAsync(request, config);
    }
}