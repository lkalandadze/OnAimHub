using GameLib.Application.Models.Configuration;
using GameLib.Domain.Entities;

namespace GameLib.Application.Services.Abstract;

public interface IGameConfigurationService
{
    Task<IEnumerable<ConfigurationBaseGetModel>> GetAllAsync();

    Task<GameConfiguration> GetByIdAsync(int id);

    Task CreateAsync(ConfigurationCreateModel model);

    Task UpdateAsync(int id, ConfigurationUpdateModel model);

    Task ActivateAsync(int id);

    Task DeactivateAsync(int id);

    Task AssignConfigurationToSegmentsAsync(int configurationId, IEnumerable<string> segmentIds);

    Task UnassignConfigurationToSegmentsAsync(int configurationId, IEnumerable<string> segmentIds);
}