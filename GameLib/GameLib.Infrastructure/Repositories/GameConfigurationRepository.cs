using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.Domain.Entities;
using Shared.Lib.Helpers;
using System;
using System.Collections;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

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
        var type = RepositoryHelper.GetDbSetEntityType<GameConfiguration>(context);

        var existingConfig = await dbSet.IncludeNotHiddenAll(type)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == updatedConfig.Id);

        if (existingConfig == null)
        {
            throw new KeyNotFoundException($"Entity with ID {updatedConfig.Id} not found.");
        }

        UpdateEntityTreeRecursive(existingConfig, updatedConfig);

        base.Update(existingConfig);
    }

    private void UpdateEntityTreeRecursive(object existingEntity, object updatedEntity)
    {
        var entityType = existingEntity.GetType();
        var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            //Temp
            if (property.Name == "Prices")
            {
                
            }

            if (property.Name == nameof(BaseEntity.Id))
            {
                continue;
            }
            else if (TypeHelper.IsGenericCollection(property.PropertyType))
            {
                var existingCollection = property.GetValue(existingEntity) as IEnumerable;
                var updatedCollection = property.GetValue(updatedEntity) as IEnumerable;

                if (existingCollection == null && updatedCollection == null)
                {
                    continue;
                }

                UpdateCollection(existingCollection.ToDynamicList(), updatedCollection.ToDynamicList());
            }
            else
            {
                property.SetValue(existingEntity, property.GetValue(updatedEntity));
            }
        }
    }

    public void UpdateCollection(List<object> oldCollection, List<object> newCollection)
    {
        var removedItems = oldCollection.Where(x => newCollection.All(xx => !HaveSameId(x, xx))).ToList();
        _context.RemoveRange(removedItems);

        var added = newCollection.Where(x =>
        {
            var id = GetId(x);
            return (id is int intId && intId == 0) || (id is string strId && string.IsNullOrEmpty(strId));
        }).ToList();
        _context.AddRange(added);

        var updatedInNew = newCollection.Where(x =>
        {
            var id = GetId(x);
            return (id is int intId && intId != 0) || (id is string strId && !string.IsNullOrEmpty(strId));
        }).ToList();

        var updateInOld = oldCollection.Where(x => updatedInNew.Any(xx => HaveSameId(x, xx))).ToList();

        foreach (var item in updatedInNew)
        {
            //Temp
            var a = updateInOld.First(x => HaveSameId(x, item));
            var b = item;

            UpdateEntityTreeRecursive(a, b);
        }
    }

    private static bool HaveSameId(object entity1, object entity2)
    {
        //Temp
        var a = GetId(entity1);
        var b = GetId(entity2);

        return a == b;
    }

    private static dynamic? GetId(object entity)
    {
        var idProperty = entity.GetType().GetProperties().FirstOrDefault(x => x.Name == nameof(BaseEntity.Id));

        return idProperty?.GetValue(entity);

        //Temp
        //return (entity as BaseEntity)?.Id;
    }
}