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

    public async Task AssignConfigurationToSegments(int configurationId, IEnumerable<string> segmentIds)
    {
        var configuration = await _configurationRepository.OfIdAsync(configurationId);

        if (configuration == null)
        {
            throw new KeyNotFoundException($"Configuration not found for id: {configurationId}");
        }

        foreach (var segmentId in segmentIds)
        {
            var segment = await _segmentRepository.OfIdAsync(segmentId);

            if (segment == null)
            {
                segment = new Segment(segmentId, configurationId);

                await _segmentRepository.InsertAsync(segment);
            }

            configuration.Segments.Add(segment);
        }

        await _unitOfWork.SaveAsync();
    }
}