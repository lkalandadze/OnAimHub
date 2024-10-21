using GameLib.Application.Models.Configuration;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using GameLib.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace GameLib.Application.Services.Concrete;

public class ConfigurationService : IConfigurationService
{
    private readonly IGameConfigurationRepository _configurationRepository;
    private readonly ISegmentRepository _segmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SharedGameConfigDbContext _dbContext;

    public ConfigurationService(IGameConfigurationRepository configurationRepository, ISegmentRepository segmentRepository, IUnitOfWork unitOfWork, SharedGameConfigDbContext dbContext)
    {
        _configurationRepository = configurationRepository;
        _segmentRepository = segmentRepository;
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ConfigurationGetModel>> GetAllAsync()
    {
        var configurations = await _configurationRepository.Query()
                                                           .Include(c => c.Segments)
                                                           .Where(c => c.Segments.Any(s => !s.IsDeleted))
                                                           .ToListAsync();

        return configurations.Select(c => ConfigurationGetModel.MapFrom(c));
    }

    public async Task<ConfigurationGetModel> GetByIdAsync(int id)
    {
        var configuration = await _configurationRepository.OfIdAsync(id);

        if (configuration == null)
        {
            throw new KeyNotFoundException($"Configuration not found for Id: {id}");
        }

        return ConfigurationGetModel.MapFrom(configuration);
    }

    public async Task CreateAsync(ConfigurationCreateModel model)
    {
        //var configuration = new GameConfiguration(model.Name, model.Value);

        //await _configurationRepository.InsertAsync(configuration);
        //await _unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(int id, ConfigurationUpdateModel model)
    {
        var configuration = await _configurationRepository.OfIdAsync(id);

        if (configuration == null)
        {
            throw new KeyNotFoundException($"Configuration not found for Id: {id}");
        }

        configuration.ChangeDetails(model.Name, model.Value);

        _configurationRepository.Update(configuration);
        await _unitOfWork.SaveAsync();
    }

    public async Task ActivateAsync(int id)
    {
        var configuration = await _configurationRepository.OfIdAsync(id);

        if (configuration == null)
        {
            throw new KeyNotFoundException($"Configuration not found for Id: {id}");
        }

        configuration.Activate();

        _configurationRepository.Update(configuration);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeactivateAsync(int id)
    {
        var configuration = await _configurationRepository.OfIdAsync(id);

        if (configuration == null)
        {
            throw new KeyNotFoundException($"Configuration not found for Id: {id}");
        }

        configuration.Deactivate();

        _configurationRepository.Update(configuration);
        await _unitOfWork.SaveAsync();
    }

    public async Task AssignConfigurationToSegmentsAsync(int configurationId, IEnumerable<string> segmentIds)
    {
        var configuration = await _configurationRepository.OfIdAsync(configurationId);

        if (configuration == null)
        {
            throw new KeyNotFoundException($"Configuration not found for Id: {configurationId}");
        }

        var existingSegments = _segmentRepository.Query(s => segmentIds.Contains(s.Id)).ToList();
        var missingSegmentIds = segmentIds.Except(existingSegments.Select(s => s.Id));

        if (missingSegmentIds.Any())
        {
            foreach (var segmentId in missingSegmentIds)
            {
                // Assuming you want to instantiate Segment with GameConfiguration as the type argument
                var segment = new Segment(segmentId);

                await _segmentRepository.InsertAsync(segment);
                existingSegments.Add(segment);
            }
        }

        configuration.AssignSegments(existingSegments);

        _configurationRepository.Update(configuration);
        await _unitOfWork.SaveAsync();
    }

    public async Task UnassignConfigurationToSegmentsAsync(int configurationId, IEnumerable<string> segmentIds)
    {
        var configuration = await _configurationRepository.OfIdAsync(configurationId);

        if (configuration == null)
        {
            throw new KeyNotFoundException($"Configuration not found for Id: {configurationId}");
        }

        var existingSegments = _segmentRepository.Query(s => segmentIds.Contains(s.Id));

        configuration.UnassignSegments(existingSegments);

        _configurationRepository.Update(configuration);
        await _unitOfWork.SaveAsync();
    }

    public async Task<GameConfiguration?> GetConfigurationByIdAsync(int id)
    {
        GameConfiguration? result = null;

        // Iterate over all DbSets in the context
        var dbSets = _dbContext.GetType().GetProperties()
            .Where(p => p.PropertyType.IsGenericType &&
                        p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

        foreach (var dbSetProperty in dbSets)
        {
            // Get the entity type of the DbSet (e.g., WheelConfiguration)
            var entityType = dbSetProperty.PropertyType.GetGenericArguments()[0];

            // Check if the entity type inherits from GameConfiguration
            if (typeof(GameConfiguration).IsAssignableFrom(entityType))
            {
                // Get the DbSet as IQueryable
                var dbSet = dbSetProperty.GetValue(_dbContext) as IQueryable;

                if (dbSet != null)
                {
                    // Cast to IQueryable<GameConfiguration>
                    var typedQuery = dbSet.Cast<GameConfiguration>();

                    // Include navigation properties dynamically
                    var queryWithIncludes = IncludeNavigationProperties(typedQuery);

                    // Query the DbSet for the entity with the specified ID
                    result = await queryWithIncludes.FirstOrDefaultAsync(gc => gc.Id == id);

                    // If a result is found, return it
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
        }

        return result; // Return null if no configuration was found
    }

    private IQueryable<GameConfiguration> IncludeNavigationProperties(IQueryable<GameConfiguration> query)
    {
        // Get all entity types in the model that inherit from GameConfiguration
        var derivedEntityTypes = _dbContext.Model.GetEntityTypes()
            .Where(e => typeof(GameConfiguration).IsAssignableFrom(e.ClrType) && !e.ClrType.IsAbstract);

        // Include navigation properties for each derived type
        foreach (var entityType in derivedEntityTypes)
        {
            // Get all navigation properties for the derived entity
            var navigationProperties = entityType.GetNavigations().Select(n => n.Name).Distinct();

            // Include each navigation property dynamically
            foreach (var navigationProperty in navigationProperties)
            {
                query = query.Include(navigationProperty);
            }
        }

        return query;
    }
}