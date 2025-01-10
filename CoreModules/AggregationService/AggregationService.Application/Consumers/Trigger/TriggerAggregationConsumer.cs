using AggregationService.Application.Services.Abstract;
using AggregationService.Domain.Abstractions.Repository;
using MassTransit;
using Shared.IntegrationEvents.IntegrationEvents.Aggregation;

namespace AggregationService.Application.Consumers.Trigger;

public sealed class TriggerAggregationConsumer : IConsumer<TriggerAggregationEvent>
{
    private readonly IAggregationConfigurationService _aggregationConfigurationService;

    public TriggerAggregationConsumer(IAggregationConfigurationService aggregationConfigurationService)
    {
        _aggregationConfigurationService = aggregationConfigurationService;
    }

    public async Task Consume(ConsumeContext<TriggerAggregationEvent> context)
    {
        var cancellationToken = context.CancellationToken;
        var request = context.Message;

        await _aggregationConfigurationService.ProcessPlayRequestAsync(request.PlayerId, request.CoinIn, request.Amount, request.PromotionId);
    }
}
