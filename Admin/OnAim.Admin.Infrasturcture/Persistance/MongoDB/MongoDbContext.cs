using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.Infrasturcture.Persistance.MongoDB;

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


    public IMongoCollection<AuditLog> AuditLogs { get; }
    public IMongoCollection<FailureLog> FailureLogs { get; }
    public IMongoCollection<CoinTemplate> CoinTemplates { get; }
    public IMongoCollection<PromotionViewTemplate> PromotionViews { get; }
    public IMongoCollection<PromotionTemplate> PromotionTemplates { get; }
    public IMongoCollection<LeaderboardTemplate> LeaderboardTemplates { get; }
    public IMongoCollection<LeaderboardTemplatePrize> LeaderboardTemplatePrizes { get; }
    public IMongoCollection<GameConfigurationTemplate> GameConfigurationTemplates { get; }
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
