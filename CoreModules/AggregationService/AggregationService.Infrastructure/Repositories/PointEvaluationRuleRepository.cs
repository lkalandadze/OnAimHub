using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Infrastructure.Persistance.MongoDB;

namespace AggregationService.Infrastructure.Repositories;

public class PointEvaluationRuleRepository : IPointEvaluationRuleRepository
{
    private readonly AggregationDbContext _databaseContext;

    public PointEvaluationRuleRepository(AggregationDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
}

