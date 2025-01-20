using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Services.GameServices;

public interface IGameTemplateService
{
    Task<ApplicationResult> GetGameConfigurationTemplateById(string id);
    Task<ApplicationResult> GetAllGameConfigurationTemplates(GameTemplateFilter filter);
    Task<ApplicationResult> DeleteGameConfigurationTemplate(string id);
    Task<GameConfigurationTemplate> CreateGameConfigurationTemplate(string gameName, object coinTemplate);
}
