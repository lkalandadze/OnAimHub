using GameLib.Application.Models.Configuration;
using GameLib.Application.Services.Abstract;
using GameLib.Domain.Abstractions;
using GameLib.Domain.Abstractions.Repository;
using GameLib.Domain.Entities;

namespace GameLib.Application.Services.Concrete;

public class ConfigurationService : IConfigurationService
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ISegmentRepository _segmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfigurationService(IConfigurationRepository configurationRepository, ISegmentRepository segmentRepository, IUnitOfWork unitOfWork)
    {
        _configurationRepository = configurationRepository;
        _segmentRepository = segmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ConfigurationGetModel>> GetAllAsync()
    {
        var configurations = await _configurationRepository.QueryAsync();

        return configurations.Select(c => new ConfigurationGetModel
        {
            Id = c.Id,
            Name = c.Name,
            Value = c.Value,
            IsActive = c.IsActive,
        });
    }

    public async Task<ConfigurationGetModel> GetByIdAsync(int id)
    {
        var configuration = await _configurationRepository.OfIdAsync(id);

        if (configuration == null)
        {
            throw new KeyNotFoundException($"Configuration not found for Id: {id}");
        }

        return new ConfigurationGetModel
        {
            Id = configuration.Id,
            Name = configuration.Name,
            Value = configuration.Value,
            IsActive = configuration.IsActive,
        };
    }

    public async Task CreateConfigurationAsync(ConfigurationCreateModel model)
    {
        var configuration = new Configuration(model.Name, model.Value);

        await _configurationRepository.InsertAsync(configuration);
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateConfigurationAsync(int id, ConfigurationUpdateModel model)
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
}