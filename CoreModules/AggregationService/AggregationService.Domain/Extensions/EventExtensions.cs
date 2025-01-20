using AggregationService.Domain.Entities;
using Shared.IntegrationEvents.IntegrationEvents.Aggregation;

namespace AggregationService.Domain.Extensions;

public static class EventExtensions
{
    public static bool PassesFilters(this AggregationTriggerEvent @event, AggregationConfiguration aggregationConfiguration)
    {
        return (@event.IsExternal || @event.Producer == aggregationConfiguration.EventProducer)
            && aggregationConfiguration.Filters.All(@event.PassesFilter);
    }

    public static bool PassesFilter(this AggregationTriggerEvent @event, Filter filter)
    {
        // Ensure the filter property exists in the event data
        if (!@event.Data.ContainsKey(filter.Property))
        {
            Console.WriteLine($"Filter property '{filter.Property}' not found in event data.");
            return false;
        }

        // Compare the filter value with the event data value
        var eventValue = @event.Data[filter.Property];
        var result = eventValue?.ToString() == filter.Value;
        Console.WriteLine($"Filter property: {filter.Property}, Event value: {eventValue}, Filter value: {filter.Value}, Passes: {result}");
        return result;
    }

    public static decimal ExtractValue(this AggregationTriggerEvent @event, AggregationConfiguration aggregationConfiguration)
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
