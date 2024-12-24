using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Entities;
using AggregationService.Infrastructure.Persistance.Data;

namespace AggregationService.Infrastructure.Repositories;

public class PlayerProgressRepository(AggregationServiceContext context) : BaseRepository<AggregationServiceContext, PlayerProgress>(context), IPlayerProgressRepository
{
}