using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.Infrasturcture.Repositories.Interfaces;

public interface IGameConfigurationTemplateRepository
{
    Task<List<GameConfigurationTemplate>> GetGameConfigurationTemplates();
    Task AddGameConfigurationTemplateAsync(GameConfigurationTemplate template);
    Task<GameConfigurationTemplate?> GetGameConfigurationTemplateByIdAsync(string id);
    Task<GameConfigurationTemplate?> UpdateGameConfigurationTemplateAsync(string id, GameConfigurationTemplate updated);
    Task<bool> DeleteGameConfigurationTemplateAsync(string id);
}
