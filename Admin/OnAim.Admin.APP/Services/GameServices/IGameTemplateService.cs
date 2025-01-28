using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Game;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Services.GameServices;

public interface IGameTemplateService
{
    Task<ApplicationResult<GameConfigurationTemplate>> GetGameConfigurationTemplateById(string id);
    Task<ApplicationResult<PaginatedResult<GameConfigurationTemplateDto>>> GetAllGameConfigurationTemplates(GameTemplateFilter filter);
    Task<ApplicationResult<bool>> DeleteGameConfigurationTemplate(string id);
    Task<GameConfigurationTemplate> CreateGameConfigurationTemplate(string gameName, object coinTemplate);
}
