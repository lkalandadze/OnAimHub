namespace AggregationService.Domain.Abstractions.Repository;

public interface ILogEntryRepository
{
    Task LogEventAsync(string subscriber, string producer, string eventDetails);
}