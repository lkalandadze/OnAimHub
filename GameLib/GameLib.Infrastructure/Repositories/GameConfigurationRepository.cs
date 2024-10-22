using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GameLib.Infrastructure.Repositories;

public class GameConfigurationRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, GameConfiguration>(context), IGameConfigurationRepository
{
    public override async Task<GameConfiguration?> OfIdAsync(dynamic id)
    {
        int convertedId = Convert.ToInt32(id);

        return await IncludeNavigationProperties(GetDbSet()).Where(c => c.Id == convertedId)
                                                            .FirstOrDefaultAsync();
    }

    public override IQueryable<GameConfiguration> Query(Expression<Func<GameConfiguration, bool>>? expression = null)
    {
        return expression != null ? GetDbSet().Where(expression) : GetDbSet();
    }

    public override async Task<List<GameConfiguration>> QueryAsync(Expression<Func<GameConfiguration, bool>>? expression = null)
    {
        return expression != null ? GetDbSet().Where(expression).ToList() : await GetDbSet().ToListAsync();
    }

    public override Task InsertAsync(GameConfiguration aggregateRoot)
    {
        return base.InsertAsync(aggregateRoot);
    }

    public override void Update(GameConfiguration aggregateRoot)
    {
        base.Update(aggregateRoot);
    }

    public override void Delete(GameConfiguration aggregateRoot)
    {
        base.Delete(aggregateRoot);
    }

    private IQueryable<GameConfiguration> GetDbSet()
    {
        var dbSets = context.GetType().GetProperties()
            .Where(p => p.PropertyType.IsGenericType &&
                        p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

        var set = dbSets.Where(set => typeof(GameConfiguration).IsAssignableFrom(set.PropertyType.GetGenericArguments()[0]))
            .FirstOrDefault()
            ?.GetValue(context) as IQueryable;

        if (set != null)
        {
            return set.Cast<GameConfiguration>();
        }

        return Enumerable.Empty<GameConfiguration>().AsQueryable();
    }

    private IQueryable<GameConfiguration> IncludeNavigationProperties(IQueryable<GameConfiguration> query)
    {
        var derivedEntityTypes = context.Model.GetEntityTypes()
            .Where(e => typeof(GameConfiguration).IsAssignableFrom(e.ClrType) && !e.ClrType.IsAbstract);

        foreach (var entityType in derivedEntityTypes)
        {
            var navigationProperties = entityType.GetNavigations().Select(n => n.Name).Distinct();

            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);
            }
        }

        return query;
    }
}