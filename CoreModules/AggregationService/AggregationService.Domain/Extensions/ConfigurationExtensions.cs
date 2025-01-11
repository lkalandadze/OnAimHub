using AggregationService.Domain.Entities;
using AggregationService.Domain.Enum;
using Shared.IntegrationEvents.IntegrationEvents.Aggregation;

namespace AggregationService.Domain.Extensions;

public static class ConfigurationExtensions
{
    public static IEnumerable<AggregationConfiguration> Filter(this IEnumerable<AggregationConfiguration> aggregationConfigurations, TriggerAggregationEvent @event)
    {
        foreach (var config in aggregationConfigurations)
        {
            if (@event.PassesFilters(config))
            {
                yield return config;
            }
        }
    }

    private static string GenerateKeyBase(this AggregationConfiguration config, TriggerAggregationEvent @event)
    {
        return $"{config.PromotionId}_{config.Key}_{@event.CustomerId}";
    }

    public static string GenerateLockKey(this AggregationConfiguration config, TriggerAggregationEvent @event)
    {
        return config.GenerateKeyBase(@event);
    }

    public static string GenerateKeyForValue(this AggregationConfiguration config, TriggerAggregationEvent @event)
    {
        return $"{config.GenerateKeyBase(@event)}_eventValue";
    }

    public static string GenerateKeyForPoints(this AggregationConfiguration config, TriggerAggregationEvent @event)
    {
        return $"{config.GenerateKeyBase(@event)}_points";
    }

    public static int CalculatePoints(this AggregationConfiguration config, double currentTotal)
    {
        if (config.EvaluationType == EvaluationType.SingleRule)
        {
            var rule = config.PointEvaluationRules.First();

            return (int)Math.Floor((double)currentTotal / rule.Step) * rule.Point;
        }

        if (config.EvaluationType == EvaluationType.Steps)
        {
            return config.PointEvaluationRules.CurrentPoints(currentTotal);
        }

        return 0;
    }
}
