using GameLib.Application.Models.Configuration;

namespace GameLib.Application.Services.Abstract;

public interface IConfigurationService
{
    Task<IEnumerable<ConfigurationGetModel>> GetAllAsync();

    Task<ConfigurationGetModel> GetByIdAsync(int id);

    Task CreateConfigurationAsync(ConfigurationCreateModel model);

    Task UpdateConfigurationAsync(int id, ConfigurationUpdateModel model);

    Task ActivateAsync(int id);

    Task DeactivateAsync(int id);

    Task AssignConfigurationToSegmentsAsync(int configurationId, IEnumerable<string> segmentIds);

    Task UnassignConfigurationToSegmentsAsync(int configurationId, IEnumerable<string> segmentIds);
}