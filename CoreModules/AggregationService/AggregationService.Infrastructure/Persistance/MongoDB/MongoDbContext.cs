using AggregationService.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AggregationService.Infrastructure.Persistance.MongoDB;

public abstract class MongoDbContext
{
    protected readonly MongoDbOptions _mongoDbOptions;
    protected readonly IMongoDatabase _database;
    protected readonly IMongoClient _client;


    public MongoDbContext(IOptions<MongoDbOptions> mongoDbOptions)
    {
        _mongoDbOptions = mongoDbOptions.Value;
        MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(_mongoDbOptions.Connection));
        _client = new MongoClient(settings);
        _database = _client.GetDatabase(_mongoDbOptions.DatabaseName);
    }


    public IMongoCollection<AggregationConfiguration> AggregationConfigurations { get; }
    public IMongoCollection<Filter> Filters { get; }
    public IMongoCollection<PointEvaluationRule> PointEvaluationRules { get; }
    public IMongoCollection<LogEntry> LogEntry { get; }
    public IMongoCollection<TEntity> GetCollection<TEntity>(string name = "")
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(TEntity).Name + "s";

        return _database.GetCollection<TEntity>(name);
    }

    public void DropDatabase()
    {
        _client.DropDatabase(_mongoDbOptions.DatabaseName);
    }

}
