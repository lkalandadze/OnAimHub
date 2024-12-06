using GameLib.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Reflection;
using TestWheel.Domain.Abstractions.Repository;
using TestWheel.Domain.Entities;
using TestWheel.Infrastructure.DataAccess;

namespace Wheel.Infrastructure.Repositories;

public class TestWheelConfigurationRepository : BaseRepository<TestWheelConfigDbContext, TestWheelConfiguration>, ITestWheelConfigurationRepository
{
    private readonly TestWheelConfigDbContext _context;

    public TestWheelConfigurationRepository(TestWheelConfigDbContext context) : base(context)
    {
        _context = context;
    }

    public DbContext GetDbContext()
    {
        return _context;
    }

    public void UpdateEntity(TestWheelConfiguration existingEntity, TestWheelConfiguration updatedEntity)
    {
        var properties = typeof(TestWheelConfiguration).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (IsCollection(property.PropertyType))
            {
                var existingCollection = property.GetValue(existingEntity) as IEnumerable;
                var updatedCollection = property.GetValue(updatedEntity) as IEnumerable;

                UpdateCollection(existingCollection, updatedCollection, property.PropertyType);
            }
            else
            {
                var updatedValue = property.GetValue(updatedEntity);
                if (updatedValue != null)
                {
                    property.SetValue(existingEntity, updatedValue);
                }
            }
        }
    }
    public void UpdateCollection(IEnumerable existingCollection, IEnumerable updatedCollection, Type collectionType)
    {
        if (existingCollection == null || updatedCollection == null) return;

        var dbContext = this.GetDbContext(); // Get the current DbContext
        var itemType = collectionType.GetGenericArguments().FirstOrDefault();
        if (itemType == null) return;

        // Clear the existing collection using reflection
        var clearMethod = typeof(ICollection<>).MakeGenericType(itemType).GetMethod("Clear");
        clearMethod?.Invoke(existingCollection, null);

        // Add the updated items to the existing collection
        var addMethod = typeof(ICollection<>).MakeGenericType(itemType).GetMethod("Add");

        foreach (var updatedItem in updatedCollection)
        {
            var entityEntry = dbContext.Entry(updatedItem);

            if (entityEntry.State == EntityState.Detached)
            {
                dbContext.Attach(updatedItem); // Attach the entity if it's not already tracked
            }

            entityEntry.State = EntityState.Modified; // Mark the entity as modified

            addMethod?.Invoke(existingCollection, new[] { updatedItem });
        }
    }

    public bool IsCollection(Type type)
    {
        return type.IsGenericType && (typeof(ICollection<>).IsAssignableFrom(type.GetGenericTypeDefinition()) || typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()));
    }
}