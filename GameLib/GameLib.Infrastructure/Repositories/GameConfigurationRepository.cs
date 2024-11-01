using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Shared.Lib.Helpers;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace GameLib.Infrastructure.Repositories;

public class GameConfigurationRepository(SharedGameConfigDbContext context) : BaseRepository<SharedGameConfigDbContext, GameConfiguration>(context), IGameConfigurationRepository
{
    public override async Task<GameConfiguration?> OfIdAsync(dynamic id)
    {
        int convertedId = Convert.ToInt32(id);
        var dbSet = RepositoryHelper.GetDbSet<GameConfiguration>(context);
        var type = RepositoryHelper.GetDbSetEntityType<GameConfiguration>(context);

        return await dbSet.IncludeNotHiddenAll(type).Where(c => c.Id == convertedId)
                                                    .FirstOrDefaultAsync();
    }

    public override IQueryable<GameConfiguration> Query(Expression<Func<GameConfiguration, bool>>? expression = null)
    {
        var dbSet = RepositoryHelper.GetDbSet<GameConfiguration>(context);

        return expression != null ? dbSet.Where(expression) : dbSet;
    }

    public override async Task<List<GameConfiguration>> QueryAsync(Expression<Func<GameConfiguration, bool>>? expression = null)
    {
        return await Query(expression).ToListAsync();
    }

    public void InsertConfigurationTree(GameConfiguration aggregateRoot)
    {
        var dbSet = RepositoryHelper.GetDbSet<GameConfiguration>(context);

        var addMethod = dbSet.GetType().GetMethod(nameof(DbSet<object>.Add));

        if (addMethod == null)
        {
            throw new ArgumentNullException(nameof(addMethod));
        }

        var parameters = new object[] { aggregateRoot };

        addMethod.Invoke(dbSet, parameters);
    }

    public async Task UpdateConfigurationTreeAsync(GameConfiguration updatedConfig)
    {
        var dbSet = RepositoryHelper.GetDbSet<GameConfiguration>(context);

        var existingConfig = await RepositoryHelper.IncludeNavigationProperties(context, dbSet)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == updatedConfig.Id);

        if (existingConfig == null)
        {
            throw new KeyNotFoundException($"Entity with ID {updatedConfig.Id} not found.");
        }

        var configType = RepositoryHelper.GetDbSetEntityType<GameConfiguration>(context);
        var properties = configType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (TypeHelper.IsGenericCollection(property.PropertyType))
            {
                var existingCollection = property.GetValue(existingConfig) as IEnumerable;
                var updatedCollection = property.GetValue(updatedConfig) as IEnumerable;

                if (existingCollection != null && updatedCollection != null)
                {
                    RepositoryHelper.UpdateCollection(context, existingCollection, updatedCollection, property.PropertyType);
                }
            }
            else
            {
                var updatedValue = property.GetValue(updatedConfig);

                if (updatedValue != null)
                {
                    property.SetValue(existingConfig, updatedValue);
                }
            }
        }

        base.Update(existingConfig);
    }
}