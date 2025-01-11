using AggregationService.Domain.Entities;
using Shared.IntegrationEvents.IntegrationEvents.Aggregation;

namespace AggregationService.Domain.Extensions;

public static class EventExtensions
{
    public static bool PassesFilters(this TriggerAggregationEvent @event, AggregationConfiguration aggregationConfiguration)
    {
        return (@event.IsExternal || @event.Producer == aggregationConfiguration.EventProducer)
            && aggregationConfiguration.Filters.All(@event.PassesFilter);
    }

    public static bool PassesFilter(this TriggerAggregationEvent @event, Filter filter)
    {
        return @event.Data.ContainsKey(filter.Property)
            && @event.Data[filter.Property] == filter.Value;
    }

    public static decimal ExtractValue(this TriggerAggregationEvent @event, AggregationConfiguration aggregationConfiguration)
    {
        if (!@event.PassesFilters(aggregationConfiguration))
        {
            return 0;
        }

        return @event.Data.ContainsKey(aggregationConfiguration.SelectionField)
            ? Convert.ToDecimal(@event.Data[aggregationConfiguration.SelectionField])
            : 0;
    }
}
