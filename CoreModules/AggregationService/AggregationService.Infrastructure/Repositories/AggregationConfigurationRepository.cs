using AggregationService.Domain.Abstractions.Repository;
using AggregationService.Domain.Entities;
using AggregationService.Infrastructure.Persistance.Data;
using MongoDB.Driver;

namespace AggregationService.Infrastructure.Repositories;

public class AggregationConfigurationRepository : BaseRepository<AggregationServiceContext, AggregationConfiguration>, IAggregationConfigurationRepository
{
    private readonly IMongoCollection<AggregationConfiguration> _collection;

    public AggregationConfigurationRepository(AggregationServiceContext context, IMongoClient mongoClient)
        : base(context)
    {
        var database = mongoClient.GetDatabase("AggregatorDB");
        _collection = database.GetCollection<AggregationConfiguration>("AggregationConfigurations");
    }

    public async Task AddConfigurationsAsync(List<AggregationConfiguration> configs)
    {
        await _collection.InsertManyAsync(configs);
    }

    public async Task<List<AggregationConfiguration>> GetAllConfigurationsAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }
}
