using OnAim.Admin.Contracts.Dtos.Game;

namespace OnAim.Admin.APP.Services.GameServices;

public interface IGameTemplateService
{
    Task<ApplicationResult<GameConfigurationTemplate>> GetGameConfigurationTemplateById(string id);
    Task<ApplicationResult<PaginatedResult<GameConfigurationTemplateDto>>> GetAllGameConfigurationTemplates(GameTemplateFilter filter);
    Task<ApplicationResult<bool>> DeleteGameConfigurationTemplate(string id);
    Task<GameConfigurationTemplate> CreateGameConfigurationTemplate(string gameName, object coinTemplate);
}
