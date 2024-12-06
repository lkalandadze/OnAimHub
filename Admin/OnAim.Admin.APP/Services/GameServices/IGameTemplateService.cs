using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Domain.Entities.Templates;

namespace OnAim.Admin.APP.Services.GameServices;

public interface IGameTemplateService
{
    Task<ApplicationResult> GetById(string id);
    Task<ApplicationResult> GetGameConfigurationTemplates();
    Task<ApplicationResult> DeleteGameConfigurationTemplate(string id);
    Task<GameConfigurationTemplate> CreateGameConfigurationTemplate(CreateGameConfigurationTemplateDto coinTemplate);
}
