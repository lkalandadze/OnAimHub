using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Entities;
using AggregationService.Infrastructure.Persistance.Data;

namespace AggregationService.Infrastructure.Repositories;

public class AggregationConfigurationRepository(AggregationServiceContext context) : BaseRepository<AggregationServiceContext, AggregationConfiguration>(context), IAggregationConfigurationRepository
{
}
