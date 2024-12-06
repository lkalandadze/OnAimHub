using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.Entities.Templates;
using OnAim.Admin.Infrasturcture.Repositories.Interfaces;

namespace OnAim.Admin.APP.Services.GameServices;

public class GameTemplateService : IGameTemplateService
{
    private readonly IGameConfigurationTemplateRepository _gameConfigurationTemplateRepository;

    public GameTemplateService(IGameConfigurationTemplateRepository gameConfigurationTemplateRepository)
    {
        _gameConfigurationTemplateRepository = gameConfigurationTemplateRepository;
    }

    public async Task<ApplicationResult> GetGameConfigurationTemplates()
    {
        var temps = await _gameConfigurationTemplateRepository.GetGameConfigurationTemplates();
        return new ApplicationResult { Data = temps, Success = true };
    }

    public async Task<ApplicationResult> GetById(string id)
    {
        var coin = await _gameConfigurationTemplateRepository.GetGameConfigurationTemplateByIdAsync(id);

        if (coin == null) throw new NotFoundException("template Not Found");

        return new ApplicationResult { Success = true, Data = coin };
    }

    public async Task<GameConfigurationTemplate> CreateGameConfigurationTemplate(CreateGameConfigurationTemplateDto coinTemplate)
    {
        var temp = new GameConfigurationTemplate
        {
            ConfigurationJson = coinTemplate.ConfigurationJson,
        };

        return temp;
    }

    public async Task<ApplicationResult> DeleteGameConfigurationTemplate(string id)
    {
        var template = await _gameConfigurationTemplateRepository.GetGameConfigurationTemplateByIdAsync(id);

        if (template == null)
        {
            throw new NotFoundException("Template Not Found");
        }

        template.Delete();

        await _gameConfigurationTemplateRepository.UpdateGameConfigurationTemplateAsync(id, template);

        return new ApplicationResult { Success = true };
    }
}
public class CreateGameConfigurationTemplateDto
{
    public string ConfigurationJson { get; set; }
}