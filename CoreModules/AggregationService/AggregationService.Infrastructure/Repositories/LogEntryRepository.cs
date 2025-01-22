using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Entities;
using AggregationService.Infrastructure.Persistance.MongoDB;

namespace AggregationService.Infrastructure.Repositories;

public class LogEntryRepository : ILogEntryRepository
{
    private readonly AggregationDbContext _databaseContext;

    public LogEntryRepository(AggregationDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    public async Task LogEventAsync(string subscriber, string producer, string eventDetails)
    {
        var logEntry = new LogEntry
        {
            Subscriber = subscriber,
            Producer = producer,
            EventDetails = eventDetails,
            Timestamp = DateTime.UtcNow
        };

        await _databaseContext.LogEntries.InsertOneAsync(logEntry);
    }
}

