using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.GameFeatures.Commands.ActivateConfiguration;
using OnAim.Admin.APP.Features.GameFeatures.Commands.CreateConfiguration;
using OnAim.Admin.APP.Features.GameFeatures.Commands.DeactivateConfiguration;
using OnAim.Admin.APP.Features.GameFeatures.Commands.UpdateConfiguration;
using OnAim.Admin.APP.Features.GameFeatures.Queries.GetById;
using OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetConfigurationMetadata;
using OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetGameConfigurations;
using OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetGameConfigurations.GetConfiguration;
using OnAim.Admin.APP.Features.GameFeatures.Template.Commands.Create;
using OnAim.Admin.APP.Features.GameFeatures.Template.Commands.Delete;
using OnAim.Admin.APP.Features.GameFeatures.Template.Queries.GetAll;
using OnAim.Admin.APP.Services.Game;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Game;

namespace OnAim.Admin.API.Controllers;

public class GamesController : ApiControllerBase
{
    private readonly IGameService _gameService;
    private readonly IGameTemplateService _gameTemplateService;

    public GamesController(IGameService gameService, IGameTemplateService gameTemplateService)
    {
        _gameService = gameService;
        _gameTemplateService = gameTemplateService;
    }

    [HttpGet(nameof(GetAll))]
    public async Task<IActionResult> GetAll([FromQuery] FilterGamesDto filter)
        => Ok(await _gameService.GetAll(filter));

    [HttpGet(nameof(GetById) + "/{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
        => Ok(await Mediator.Send(new GetGameQuery(id)));

    [HttpGet(nameof(GetConfiguration))]
    public async Task<IActionResult> GetConfiguration([FromQuery] int id)
        => Ok(await Mediator.Send(new GetConfigurationQuery(id)));

    [HttpGet(nameof(GetConfigurations))]
    public async Task<IActionResult> GetConfigurations()
        => Ok(await Mediator.Send(new GetGameConfigurationsQuery()));

    [HttpGet(nameof(GetConfigurationMetadata))]
    public async Task<IActionResult> GetConfigurationMetadata()
        => Ok(await Mediator.Send(new GetConfigurationMetadataQuery()));

    [HttpPost(nameof(CreateConfiguration))]
    public async Task<IActionResult> CreateConfiguration([FromQuery] string gameName, [FromBody] GameConfigurationDto configurationJson)
        => Ok(await Mediator.Send(new CreateConfigurationCommand(gameName, configurationJson)));

    [HttpPut(nameof(UpdateConfiguration))]
    public async Task<IActionResult> UpdateConfiguration([FromQuery] string gameName, [FromBody] GameConfigurationDto configurationJson)
        => Ok(await Mediator.Send(new UpdateConfigurationCommand(gameName, configurationJson)));

    [HttpPut(nameof(ActivateConfiguration))]
    public async Task<IActionResult> ActivateConfiguration([FromQuery] int id)
        => Ok(await Mediator.Send(new ActivateConfigurationCommand(id)));

    [HttpPut(nameof(DeactivateConfiguration))]
    public async Task<IActionResult> DeactivateConfiguration([FromQuery] int id)
        => Ok(await Mediator.Send(new DeactivateConfigurationCommand(id)));

    //Game Configuration Template

    [HttpPost(nameof(CreateGameConfigurationTemplate))]
    public async Task<IActionResult> CreateGameConfigurationTemplate([FromBody] CreateGameConfigurationTemplateCommand command)
        => Ok(await Mediator.Send(command));

    [HttpDelete(nameof(DeleteGameConfigurationTemplate))]
    public async Task<IActionResult> DeleteGameConfigurationTemplate([FromQuery] DeleteGameConfigurationTemplateCommand command)
        => Ok(await Mediator.Send(command));

    [HttpGet(nameof(GetAllGameConfigurationTemplates))]
    public async Task<IActionResult> GetAllGameConfigurationTemplates([FromQuery] BaseFilter filter)
        => Ok(await Mediator.Send(new GetAllGameConfigurationTemplatesQuery(filter)));

    [HttpGet(nameof(GetGameConfigurationTemplateById))]
    public async Task<IActionResult> GetGameConfigurationTemplateById([FromQuery] string id)
    => Ok(await _gameTemplateService.GetGameConfigurationTemplateById(id));
}
