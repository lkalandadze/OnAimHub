using GameLib.Application.Generators;
using GameLib.Application.Models.Configuration;
using GameLib.Domain.Entities;

namespace GameLib.Application.Services.Abstract;

public interface IGameConfigurationService
{
    EntityMetadata? GetConfigurationMetaData();

    Task<IEnumerable<ConfigurationBaseGetModel>> GetAllAsync();

    Task<GameConfiguration> GetByIdAsync(int id);

    Task CreateAsync(string configurationJson);

    Task UpdateAsync(string configurationJson);

    Task ActivateAsync(int id);

    Task DeactivateAsync(int id);

    Task DeleteAsync(int id);
}