using AggregationService.Domain.Abstractions.Repository;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace AggregationService.Infrastructure.Persistance.MongoDB;

public class MongoDbBaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class
{
    private readonly IMongoCollection<TEntity> _collection;

    public MongoDbBaseRepository(MongoDbContext dbContext, string collectionName)
    {
        _collection = dbContext.GetCollection<TEntity>(collectionName);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> predicate)
    {
        await _collection.ReplaceOneAsync(predicate, entity, new ReplaceOptions { IsUpsert = false });
    }

    public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        await _collection.DeleteOneAsync(predicate);
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _collection.Find(predicate).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var filter = predicate ?? (_ => true);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<long> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var filter = predicate ?? (_ => true);
        return await _collection.CountDocumentsAsync(filter);
    }

    public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? predicate = null)
    {
        return predicate == null ? _collection.AsQueryable() : _collection.AsQueryable().Where(predicate);
    }

    public async Task CommitChangesAsync()
    {
        // MongoDB does not have transactions unless configured with replicas.
        await Task.CompletedTask;
    }
}