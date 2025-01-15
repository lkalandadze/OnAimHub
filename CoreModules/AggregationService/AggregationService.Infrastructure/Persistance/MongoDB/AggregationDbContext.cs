using AggregationService.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AggregationService.Infrastructure.Persistance.MongoDB;

public class AggregationDbContext : MongoDbContext
{
    public AggregationDbContext(IOptions<MongoDbOptions> options) : base(options)
    {
        AggregationConfigurations = GetCollection<AggregationConfiguration>();
        Filters = GetCollection<Filter>();
        PointEvaluationRules = GetCollection<PointEvaluationRule>();
    }

    public IMongoCollection<AggregationConfiguration> AggregationConfigurations { get; }
    public IMongoCollection<Filter> Filters { get; }
    public IMongoCollection<PointEvaluationRule> PointEvaluationRules { get; }
}
