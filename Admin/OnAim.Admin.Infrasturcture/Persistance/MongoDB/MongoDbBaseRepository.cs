using MongoDB.Driver;
using OnAim.Admin.Domain.Entities.Abstract;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Infrasturcture.Persistance.Data.Admin;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using System.Linq.Expressions;

namespace OnAim.Admin.Infrasturcture.Persistance.MongoDB;

public class MongoDbBaseRepository<TEntity> : IRepository<TEntity>
where TEntity : BaseEntity
{
    private readonly IMongoCollection<TEntity> _collection;

    public IUnitOfWork UnitOfWork => throw new NotImplementedException();

    public MongoDbBaseRepository(MongoDbContext dbContext, string collectionName = "")
    {
        _collection = dbContext.GetCollection<TEntity>(collectionName);
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        await _collection.ReplaceOneAsync(p => p.Id == entity.Id, entity, new ReplaceOptions() { IsUpsert = false });
    }

    public async Task DeleteAsync(int id)
    {
        await _collection.DeleteOneAsync(p => p.Id == id);
    }

    public virtual async Task<TEntity> GetByIdAsync(int id)
    {
        return await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetListAsync()
    {
        return await _collection.AsQueryable().ToListAsync();
    }

    public virtual async Task<long> CountAsync()
    {
        return await _collection.CountDocumentsAsync(f => true);
    }

    public void Dispose()
    {
        //   Db.Dispose();
        GC.SuppressFinalize(this);
    }

    public Task<TEntity?> Find(int uId)
    {
        throw new NotImplementedException();
    }

    public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? expression = null)
    {
        throw new NotImplementedException();
    }

    public Task Store(TEntity document)
    {
        throw new NotImplementedException();
    }

    public void Remove(TEntity document)
    {
        throw new NotImplementedException();
    }

    public void WithDbContext(DatabaseContext dbContext)
    {
        throw new NotImplementedException();
    }

    public Task CommitChanges()
    {
        throw new NotImplementedException();
    }
}
