using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Entities;
using AggregationService.Infrastructure.Persistance.MongoDB;
using MongoDB.Driver;

namespace AggregationService.Infrastructure.Repositories;

public class AggregationConfigurationRepository : IAggregationConfigurationRepository
{
    private readonly IMongoCollection<AggregationConfiguration> _collection;
    private readonly AggregationDbContext _dbContext;

    public AggregationConfigurationRepository(IMongoClient mongoClient, AggregationDbContext context)
    {
        var database = mongoClient.GetDatabase("AggregatorDB");
        _collection = database.GetCollection<AggregationConfiguration>("AggregationConfigurations");
        _dbContext = context;
    }

    public IMongoCollection<AggregationConfiguration> GetCollection()
    {
        return _collection;
    }

    public async Task AddConfigurationsAsync(List<AggregationConfiguration> configs)
    {
        await _collection.InsertManyAsync(configs);
    }

    public async Task<List<AggregationConfiguration>> GetAllConfigurationsAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }
    public async Task UpdateAsync(AggregationConfiguration aggregation, FilterDefinition<AggregationConfiguration> filter)
    {
        await _collection.ReplaceOneAsync(filter, aggregation, new ReplaceOptions { IsUpsert = false });
    }
}