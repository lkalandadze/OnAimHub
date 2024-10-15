//using Microsoft.EntityFrameworkCore.Metadata;
//using Microsoft.EntityFrameworkCore;
//using System.Reflection;
//using Wheel.Domain.Entities;

//public class SeededR<T>(T context) where T : DbContext
//{
//    public T Context { get; } = context;

//    /// <summary>
//    /// Seeds selected tntities with specified options
//    /// </summary>
//    public void Seed(params EntitySeedOptions[] typesToSeed)
//    {
//        typesToSeed = typesToSeed.Where(x => GetContextEntities().Select(x => x.ClrType).Contains(x.TargetType)).ToArray();

//        foreach (var typeToSeed in typesToSeed)
//        {

//        }
//    }

//    /// <summary>
//    /// Seeds given entities without specific options
//    /// </summary>
//    public void Seed(params Type[] types)
//    {
//        Seed(types.Select(x => new EntitySeedOptions
//        {
//            TargetType = x,
//        }).ToArray());
//    }

//    /// <summary>
//    /// Seeds only one entity without specific options
//    /// </summary>
//    public void Seed<Tentity>() where Tentity : class
//    {
//        Seed(typeof(Tentity));
//    }

//    /// <summary>
//    /// Seeds the whole db
//    /// </summary>
//    public void Seed()
//    {
//        Seed(GetContextEntities().Select(x => x.ClrType).ToArray());
//    }

//    private List<IEntityType> GetContextEntities()
//    {
//        return Context.Model.GetEntityTypes().ToList();
//    }
//}

//public class TempDbSeeder<T> where T : WheelConfigDbContext
//{
//    static Dictionary<Type, List<object>> localDb = new();

//    private static object GetRandomRow(T context, Type entityType)
//    {
//        if (!localDb.ContainsKey(entityType))
//        {
//            var set = GetSet(context, entityType);

//            var queryable = (IQueryable)set;

//            //var toListMethod = typeof(Enumerable).GetMethods()
//            //.Where(m => m.Name == nameof(Enumerable.ToList) && m.GetParameters().Length == 1)
//            //.FirstOrDefault()
//            //?.MakeGenericMethod(entityType);


//            //var resultList = toListMethod.Invoke(null, new object[] { queryable });

//            var random = new Random();
//            var top50 = queryable
//                .Cast<object>()
//                .Take(50)
//                .ToList();


//            localDb.Add(entityType, top50.Cast<object>().ToList());
//        }

//        return localDb[entityType][Random.Shared.Next(0, localDb[entityType].Count())];
//    }

//    private static object GetSet(T context, Type entityType)
//    {
//        var method = typeof(T).GetMethods()
//                            .Where(m => m.Name == nameof(DbContext.Set) && m.IsGenericMethod)
//                            .FirstOrDefault(m => m.GetGenericArguments().Length == 1)
//                            ?.MakeGenericMethod(entityType);

//        return method?.Invoke(context, null);
//    }

//    public static void Seed(T context)
//    {
//        //GetRandomRow(context, typeof(JackpotPrizeGroup));

//        var entityTypes = context.Model.GetEntityTypes()
//            .Where(x => x.ClrType == typeof(JackpotPrize))
//            .ToList();

//        var orderedEntityTypes = OrderEntityTypesByDependencies(entityTypes);

//        foreach (var entityType in orderedEntityTypes)
//        {
//            var entityTypeClr = entityType.ClrType;

//            var dbSet = GetSet(context, entityTypeClr);

//            if (dbSet != null)
//            {
//                SeedEntity(context, dbSet, entityTypeClr);
//            }
//        }
//    }

//    private static void SeedEntity(T context, object dbSet, Type entityType)
//    {
//        var instance = Activator.CreateInstance(entityType);
//        var addMethod = dbSet.GetType().GetMethod("Add");

//        foreach (var property in entityType.GetProperties())
//        {
//            if (property.Name == "Id")
//            {
//                continue;
//                //what if is not autoincrement?
//            }
//            if (property.PropertyType == typeof(int) && property.Name.EndsWith("Id"))
//            {
//                continue;
//                //// Set foreign key property
//                //var foreignKeyValue = GetForeignKeyValue(context, property, entityType);
//                //property.SetValue(instance, foreignKeyValue);
//            }
//            if (property.PropertyType.IsClass && property.Name != "Id")
//            {
//                continue;
//                //if (entityType == typeof(JackpotPrize))
//                //{
//                //    if (property.PropertyType == typeof(PrizeType))
//                //    {
//                //        property.SetValue(instance, GetRandomRow(context, property.PropertyType));
//                //    }
//                //}
//                //else
//                //{
//                //    property.SetValue(instance, GetRandomRow(context, property.PropertyType));
//                //}
//            }
//            else if (property.PropertyType == typeof(int))
//            {
//                property.SetValue(instance, new Random().Next(1, 1000));
//            }
//            else if (property.PropertyType == typeof(string))
//            {
//                property.SetValue(instance, $"Random_{Guid.NewGuid().ToString().Substring(0, 8)}");
//            }
//            else if (property.PropertyType == typeof(bool))
//            {
//                property.SetValue(instance, new Random().Next(2) == 0);
//            }
//            else if (property.PropertyType == typeof(List<int>))
//            {
//                var random = new Random();
//                var listSize = random.Next(1, 5);
//                var intList = Enumerable.Range(1, listSize).Select(_ => random.Next(1, 100)).ToList();
//                property.SetValue(instance, intList);
//            }

//            if (entityType == typeof(JackpotPrize))
//            {
//                var foreignKeys = context.Model.FindEntityType(typeof(JackpotPrize))?.GetForeignKeys();

//                foreach (var foreignKey in foreignKeys)
//                {
//                    var foreignType = foreignKey.PrincipalEntityType;
//                    //var foreignRow = GetRandomRow(context, foreignType.ClrType);
//                    //var principalPkName = foreignType.FindDeclaredPrimaryKey();

//                    if (property.PropertyType == foreignType)
//                    {
//                        //property.SetValue(instance, foreignRow);
//                    }
//                }
//            }
//        }

//        //if (entityType == typeof(JackpotPrize))
//        //{
//        //    var foreignKeys = context.Model.FindEntityType(typeof(JackpotPrize))?.GetForeignKeys();

//        //    foreach (var foreignKey in foreignKeys)
//        //    {
//        //        var foreignType = foreignKey.PrincipalEntityType;
//        //        var foreignRow = GetRandomRow(context, foreignType.ClrType);
//        //        var principalPkName = foreignType.FindDeclaredPrimaryKey();

//        //    }

//        //    var entityType1 = context.Model.FindEntityType(typeof(JackpotPrize));

//        //    //var relationships = entityType1.GetNavigations();

//        //    //foreach (var navigation in relationships)
//        //    //{
//        //    //    var foreignType = navigation.TargetEntityType;

//        //    //var foreignKeys = foreignType.GetForeignKeys();

//        //    //foreach (var fk in foreignKeys)
//        //    //{
//        //    //    if (fk.PrincipalEntityType == entityType1)
//        //    //    {
//        //    //        Console.WriteLine($"Foreign Key: {fk.Properties.First().Name} -> {fk.PrincipalEntityType.ClrType.Name}");
//        //    //    }
//        //    //}
//        //    //}

//        //    //var j = new JackpotPrize().GetType().GetProperties().ToList();
//        //    //var j1 = Activator.CreateInstance(typeof(JackpotPrize));

//        //    //addMethod.Invoke(context.JackpotPrizes, [instance]);
//        //    //addMethod.Invoke(dbSet, [new JackpotPrize { PrizeGroupId = 1, PrizeTypeId = 1 }]);

//        //    //(instance as JackpotPrize).PrizeGroupId = context.JackpotPrizeGroups.First().Id;
//        //    //(instance as JackpotPrize).PrizeGroup = context.JackpotPrizeGroups.First(); 
//        //    addMethod.Invoke(dbSet, [instance]);
//        //}
//        //else
//        //{
//        //    addMethod.Invoke(dbSet, [instance]);
//        //}

//        addMethod.Invoke(dbSet, [instance]);
//        AddRow(entityType, instance);
//        context.SaveChanges();
//    }

//    private static void AddRow(Type entityType, object? row)
//    {
//        if (!localDb.ContainsKey(entityType))
//        {
//            localDb.Add(entityType, new List<object>());
//        }

//        localDb[entityType].Add(row);
//    }

//    private static int GetForeignKeyValue(T context, PropertyInfo property, Type entityType)
//    {
//        // Determine the related entity type name (e.g., if the FK property is "CategoryId", this would be "Category")
//        var relatedEntityTypeName = property.Name.Replace("Id", "");

//        // Get the related entity type from the model
//        var relatedEntityType = context.Model.GetEntityTypes().FirstOrDefault(t => t.ClrType.Name == relatedEntityTypeName);

//        if (relatedEntityType != null)
//        {
//            // Get the DbSet for the related entity type
//            var relatedDbSet = typeof(T).GetMethods()
//                                    .Where(m => m.Name == nameof(DbContext.Set) && m.IsGenericMethod)
//                                    .FirstOrDefault(m => m.GetGenericArguments().Length == 1)
//                                    ?.MakeGenericMethod(relatedEntityType.ClrType)
//                                    .Invoke(context, null);

//            if (relatedDbSet != null)
//            {
//                // Cast the DbSet to IQueryable to use LINQ methods
//                var queryable = relatedDbSet as IQueryable<object>;

//                // Use LINQ to retrieve the first entity from the related DbSet
//                var relatedEntity = queryable?.FirstOrDefault();

//                if (relatedEntity != null)
//                {
//                    // Get the "Id" property from the related entity
//                    var relatedIdProperty = relatedEntityType.ClrType.GetProperty("Id");

//                    if (relatedIdProperty != null)
//                    {
//                        // Return the value of the "Id" property
//                        return (int)relatedIdProperty.GetValue(relatedEntity);
//                    }
//                }
//            }
//        }

//        // Default value if no valid ID is found
//        return 1;
//    }

//    private static List<IEntityType> OrderEntityTypesByDependencies(List<IEntityType> entityTypes)
//    {
//        var orderedEntities = new List<IEntityType>();
//        var unprocessedEntities = new HashSet<IEntityType>(entityTypes);

//        while (unprocessedEntities.Count > 0)
//        {
//            var entitiesToProcess = unprocessedEntities
//                .Where(e => !e.GetForeignKeys().Any(fk => unprocessedEntities.Contains(fk.PrincipalEntityType)))
//                .ToList();

//            if (entitiesToProcess.Count == 0)
//            {
//                throw new InvalidOperationException("Cyclic dependency detected among the entity types.");
//            }

//            orderedEntities.AddRange(entitiesToProcess);
//            entitiesToProcess.ForEach(e => unprocessedEntities.Remove(e));
//        }

//        return orderedEntities;
//    }
//}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections;
using System.Reflection;

public class SeededR<T> where T : DbContext
{
    public T Context { get; }
    private Dictionary<Type, List<object>> localDb; // Cache for storing random records for foreign tables

    public SeededR(T context)
    {
        Context = context;
        localDb = new Dictionary<Type, List<object>>();
    }


    /// <summary>
    /// Seeds given entities without specific options
    /// </summary>
    public void Seed(params Type[] types)
    {
        Seed(types.Select(x => new EntitySeedOptions
        {
            TargetType = x,
        }).ToArray());
    }

    /// <summary>
    /// Seeds only one entity without specific options
    /// </summary>
    public void Seed<Tentity>() where Tentity : class
    {
        Seed(typeof(Tentity));
    }

    /// <summary>
    /// Seeds the whole db
    /// </summary>
    public void Seed()
    {
        Seed(GetContextEntities().Select(x => x.ClrType).ToArray());
    }

    /// <summary>
    /// Seeds selected entities with specified options
    /// </summary>
    public void Seed(params EntitySeedOptions[] typesToSeed)
    {
        // Filter only valid entities within the context
        typesToSeed = typesToSeed
            .Where(x => GetContextEntities()
            .Select(e => e.ClrType)
            .Contains(x.TargetType))
            .ToArray();

        // Sort entities by dependency (you can use the dependency sorting logic here if required)
        var sortedTypesToSeed = SortEntitiesByDependency(typesToSeed);

        // Seed each entity type with the options provided
        foreach (var typeToSeed in sortedTypesToSeed)
        {
            SeedEntity(typeToSeed);
        }

        Context.SaveChanges();
    }

    /// <summary>
    /// Seeds a specific entity type based on the options provided.
    /// </summary>
    private void SeedEntity(EntitySeedOptions seedOptions)
    {
        var entityType = seedOptions.TargetType;
        var properties = entityType.GetProperties();

        // Create an empty list of the entity type
        var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(entityType));

        // Generate entities with specified property seed options
        for (int i = 0; i < seedOptions.SeedCount; i++)
        {
            var entity = Activator.CreateInstance(entityType);

            foreach (var property in properties)
            {
                if (property.Name == "Id" || property.Name.EndsWith("Id") || IsNavigationProperty(property, entityType))
                {
                    // Skip 'Id' properties and navigation properties (leave them unchanged)
                    continue;
                }

                // If the property is a foreign key, get a random record from the foreign table
                if (IsForeignKey(property, entityType, out var foreignEntityType, out var foreignKeyProperty))
                {
                    var randomForeignEntity = GetRandomForeignEntity(foreignEntityType);
                    property.SetValue(entity, foreignKeyProperty.GetValue(randomForeignEntity));
                }
                else
                {
                    // Set property values based on PropertySeedOption
                    var propOption = seedOptions.PropertySeedOptions?.FirstOrDefault(p => p.PropertyName == property.Name);
                    if (propOption != null)
                    {
                        var value = GetPropertyValue(propOption);
                        property.SetValue(entity, value);
                    }
                }
            }

            // Add entity to the list and also to localDb
            list.Add(entity);
            AddEntityToLocalDb(entityType, entity);
        }

        // Add the generated entities to the context
        Context.AddRange(list);
    }

    /// <summary>
    /// Check if the property is a navigation property (related to another entity).
    /// </summary>
    private bool IsNavigationProperty(PropertyInfo property, Type entityType)
    {
        var entityTypeMetadata = Context.Model.FindEntityType(entityType);
        return entityTypeMetadata?.FindNavigation(property.Name) != null;
    }

    /// <summary>
    /// Check if the property is a foreign key, and return the foreign entity type and its primary key property.
    /// </summary>
    private bool IsForeignKey(PropertyInfo property, Type entityType, out Type foreignEntityType, out PropertyInfo foreignKeyProperty)
    {
        foreignEntityType = null;
        foreignKeyProperty = null;

        // Get the EF Core metadata for the entity
        var entityTypeMetadata = Context.Model.FindEntityType(entityType);
        if (entityTypeMetadata == null)
            return false;

        // Get the EF property metadata corresponding to the PropertyInfo
        var efProperty = entityTypeMetadata.FindProperty(property.Name);
        if (efProperty == null)
            return false;

        // Check if the property has any foreign key relationships
        var foreignKey = entityTypeMetadata.FindForeignKeys(efProperty).FirstOrDefault();
        if (foreignKey != null)
        {
            foreignEntityType = foreignKey.PrincipalEntityType.ClrType; // Get the related entity type
            var primaryKey = foreignKey.PrincipalKey.Properties.FirstOrDefault(); // Get the primary key of the related entity
            if (primaryKey != null)
            {
                foreignKeyProperty = foreignEntityType.GetProperty(primaryKey.Name); // Find the property in the related entity
                return true;
            }
        }

        return false;
    }

    private object GetSet(Type entityType)
    {
        var method = typeof(T).GetMethods()
                            .Where(m => m.Name == nameof(DbContext.Set) && m.IsGenericMethod)
                            .FirstOrDefault(m => m.GetGenericArguments().Length == 1)
                            ?.MakeGenericMethod(entityType);



        return method?.Invoke(Context, null);
    }


    /// <summary>
    /// Get a random record from the foreign table, either from localDb or by loading from the database.
    /// </summary>
    private object GetRandomForeignEntity(Type foreignEntityType)
    {
        if (!localDb.ContainsKey(foreignEntityType))
        {
            var toListMethod = typeof(Enumerable).GetMethods();

            var random = new Random();
            var top50 = (GetSet(foreignEntityType) as IQueryable)
                .Cast<object>()
                .Take(50)
                .ToList();

            localDb[foreignEntityType] = top50.Cast<object>().ToList();
        }

        var cachedRecords = localDb[foreignEntityType];
        var randomRecord = cachedRecords[new Random().Next(cachedRecords.Count)];
        return randomRecord;
    }

    /// <summary>
    /// Add a newly created entity to localDb for reuse when seeding other entities.
    /// </summary>
    private void AddEntityToLocalDb(Type entityType, object entity)
    {
        if (!localDb.ContainsKey(entityType))
        {
            localDb[entityType] = new List<object>();
        }

        localDb[entityType].Add(entity);
    }

    /// <summary>
    /// Get value for a property based on the specified seed option.
    /// </summary>
    private object GetPropertyValue(PropertySeedOption propOption)
    {
        switch (propOption.PropertySeedOptionType)
        {
            case PropertySeedOptionType.Single:
                return propOption.Values.FirstOrDefault();
            case PropertySeedOptionType.InNumberRange:
                var random = new Random();
                var min = Convert.ToInt32(propOption.Values.First());
                var max = Convert.ToInt32(propOption.Values.Last());
                return random.Next(min, max);
            case PropertySeedOptionType.FromList:
                return propOption.Values[new Random().Next(propOption.Values.Count)];
            case PropertySeedOptionType.Random:
                return new Random().Next();
            default:
                return null;
        }
    }

    /// <summary>
    /// Get all the entities from the current context
    /// </summary>
    private List<IEntityType> GetContextEntities()
    {
        return Context.Model.GetEntityTypes().ToList();
    }

    /// <summary>
    /// Sorts the entities by dependency (Foreign key relationships etc.)
    /// </summary>
    private EntitySeedOptions[] SortEntitiesByDependency(EntitySeedOptions[] typesToSeed)
    {
        // Placeholder for actual dependency sorting logic
        return typesToSeed;
    }
}
