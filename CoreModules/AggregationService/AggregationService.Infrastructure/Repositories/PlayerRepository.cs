using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Entities;
using AggregationService.Infrastructure.Persistance.Data;

namespace AggregationService.Infrastructure.Repositories;

public class PlayerRepository(AggregationServiceContext context) : BaseRepository<AggregationServiceContext, Player>(context), IPlayerRepository
{
}
