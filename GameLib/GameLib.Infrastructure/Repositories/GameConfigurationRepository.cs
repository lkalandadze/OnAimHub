using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.Domain.Entities;
using Shared.Lib.Helpers;
using System.Collections;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

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

        UpdateEntityTree(existingConfig, updatedConfig);

        base.Update(existingConfig);
    }

    private void UpdateEntityTree(object existingEntity, object updatedEntity)
    {
        var entityType = existingEntity.GetType();
        var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (TypeHelper.IsGenericCollection(property.PropertyType))
            {
                var existingCollection = property.GetValue(existingEntity) as IEnumerable;
                var updatedCollection = property.GetValue(updatedEntity) as IEnumerable;

                UpdateCollection(existingCollection.ToDynamicList(), updatedCollection.ToDynamicList());
            }
            //else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            //{
            //    continue;
            //    // Review
            //    //var existingChild = property.GetValue(existingEntity);
            //    //var updatedChild = property.GetValue(updatedEntity);

            //    //if (updatedChild != null)
            //    //{
            //    //    if (existingChild != null)
            //    //    {
            //    //        UpdateEntityTree(existingChild, updatedChild);
            //    //    }
            //    //    else
            //    //    {
            //    //        property.SetValue(existingEntity, updatedChild);
            //    //    }
            //    //}
            //}
            else
            {
                property.SetValue(existingEntity, property.GetValue(updatedEntity));
            }
        }
    }

    public void UpdateRecursive(object existingEntity, object updatedEntity)
    {
        var entityType = existingEntity.GetType();
        var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        if (entityType != updatedEntity.GetType())
        {
            throw new Exception();
        }

        foreach (var property in properties)
        {
            if (TypeHelper.IsGenericCollection(property.PropertyType))
            {
                //UpdateCollection();
            }
            else
            {
                property.SetValue(existingEntity, property.GetValue(updatedEntity));
            }
        }
    }

    public void UpdateCollection(List<object> oldCollection, List<object> newCollection)
    {
        var removedItems = oldCollection.Where(x => newCollection.All(xx => HaveSameId(x,xx))).ToList();
        _context.RemoveRange(removedItems);

        var added = newCollection.Where(x => GetId(x) == 0).ToList();
        _context.AddRange(added);

        var updatedInNew = newCollection.Where(x => GetId(x) != 0).ToList();
        var updateInOld = oldCollection.Where(x => newCollection.Any(xx => HaveSameId(x, xx))).ToList();

        foreach (var item in updatedInNew)
        {
            var a = updateInOld.First(x => GetId(x));
            var b = item;

            UpdateEntityTree(a, b);
        }
    }

    private static bool HaveSameId(object entity1, object entity2)
    {
        var a = GetId(entity1);
        var b = GetId(entity2);

        return a == b;
    }

    private static dynamic? GetId(object entity)
    {
        return (entity as BaseEntity)?.Id;
    }
}