using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Shared.Lib.Helpers;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

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

    public void DeleteConfigurationTree(GameConfiguration aggregateRoot)
    {
        var dbSet = RepositoryHelper.GetDbSet<GameConfiguration>(context);
        var removeMethod = dbSet.GetType().GetMethod(nameof(DbSet<object>.Remove));

        if (removeMethod == null)
        {
            throw new ArgumentNullException(nameof(removeMethod));
        }

        var parameters = new object[] { aggregateRoot };

        removeMethod.Invoke(dbSet, parameters);
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
        var type = RepositoryHelper.GetDbSetEntityType<GameConfiguration>(context);

        var existingConfig = await dbSet.IncludeNotHiddenAll(type)
            .FirstOrDefaultAsync(x => x.Id == updatedConfig.Id);

        if (existingConfig == null)
        {
            throw new KeyNotFoundException($"Entity with ID {updatedConfig.Id} not found.");
        }

        RepositoryHelper.UpdateEntityTreeRecursive(context, existingConfig, updatedConfig);

        base.Update(existingConfig);
    }
}