using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Infrastructure.Persistance.Data;

namespace AggregationService.Infrastructure.Repositories;

public class FilterRepository(AggregationServiceContext context) : BaseRepository<AggregationServiceContext, Filter>(context), IFilterRepository
{
}
