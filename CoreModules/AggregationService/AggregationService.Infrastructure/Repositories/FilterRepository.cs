using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Infrastructure.Persistance.MongoDB;

namespace AggregationService.Infrastructure.Repositories;

public class FilterRepository : IFilterRepository
{
    private readonly AggregationDbContext _databaseContext;

    public FilterRepository(AggregationDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
}

