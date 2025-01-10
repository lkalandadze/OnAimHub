using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Entities;
using AggregationService.Infrastructure.Persistance.Data;

namespace AggregationService.Infrastructure.Repositories;


public class PointEvaluationRuleRepository(AggregationServiceContext context) : BaseRepository<AggregationServiceContext, PointEvaluationRule>(context), IPointEvaluationRuleRepository
{
}

