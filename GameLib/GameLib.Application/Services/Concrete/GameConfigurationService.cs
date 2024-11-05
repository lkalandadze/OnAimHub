using GameLib.Application.Generators;
using GameLib.Application.Models.Configuration;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GameLib.Application.Services.Concrete;

public class GameConfigurationService : IGameConfigurationService
{
    private readonly IGameConfigurationRepository _configurationRepository;
    private readonly ISegmentRepository _segmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EntityGenerator _entityGenerator;

    public GameConfigurationService(IGameConfigurationRepository configurationRepository, ISegmentRepository segmentRepository, IUnitOfWork unitOfWork, EntityGenerator entityGenerator)
    {
        _configurationRepository = configurationRepository;
        _segmentRepository = segmentRepository;
        _unitOfWork = unitOfWork;
        _entityGenerator = entityGenerator;
    }

    public EntityMetadata? GetConfigurationMetaData()
    {
        return _entityGenerator.GenerateEntityMetadata(Globals.ConfigurationType);
    }

    public async Task<IEnumerable<ConfigurationBaseGetModel>> GetAllAsync()
    {
        var configurations = await _configurationRepository.Query().ToListAsync();
        var mappedConfigurations = configurations.Select(c => ConfigurationBaseGetModel.MapFrom(c));

        return mappedConfigurations == null ? Enumerable.Empty<ConfigurationBaseGetModel>() : mappedConfigurations;
    }

    public async Task<GameConfiguration> GetByIdAsync(int id)
    {
        var configuration = await _configurationRepository.OfIdAsync(id);

        if (configuration == null)
        {
            throw new KeyNotFoundException($"Configuration not found for Id: {id}");
        }

        return configuration;
    }

    public async Task CreateAsync(string configurationJson)
    {
        var configurationTree = JsonConvert.DeserializeObject(configurationJson, Globals.ConfigurationType) as GameConfiguration;

        if (configurationTree == null)
        {
            throw new ArgumentException("Invalid JSON configurationTree for configuration create.");
        }

        try
        {
            _configurationRepository.InsertConfigurationTree(configurationTree);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task UpdateAsync(string configurationJson)
    {
        if (string.IsNullOrWhiteSpace(configurationJson))
        {
            throw new ArgumentException("Configuration JSON cannot be null or empty.", nameof(configurationJson));
        }

        var updatedConfiguration = JsonConvert.DeserializeObject(configurationJson, Globals.ConfigurationType) as GameConfiguration;

        if (updatedConfiguration == null)
        {
            throw new ArgumentException("Invalid JSON configuration for update.");
        }

        try
        {
            await _configurationRepository.UpdateConfigurationTreeAsync(updatedConfiguration);
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
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
}