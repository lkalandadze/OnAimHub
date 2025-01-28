﻿using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.GameFeatures.Commands.ActivateConfiguration;
using OnAim.Admin.APP.Features.GameFeatures.Commands.CreateConfiguration;
using OnAim.Admin.APP.Features.GameFeatures.Commands.DeactivateConfiguration;
using OnAim.Admin.APP.Features.GameFeatures.Commands.UpdateConfiguration;
using OnAim.Admin.APP.Features.GameFeatures.Queries.GetById;
using OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetConfigurationMetadata;
using OnAim.Admin.APP.Features.GameFeatures.Template.Commands.Create;
using OnAim.Admin.APP.Features.GameFeatures.Template.Commands.Delete;
using OnAim.Admin.APP.Features.GameFeatures.Template.Queries.GetAll;
using OnAim.Admin.APP.Services.GameServices;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Game;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.Domain.Entities.Templates;

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

    #region game

    [HttpGet(nameof(GetAll))]
    public async Task<IActionResult> GetAll([FromQuery] FilterGamesDto filter)
        => Ok(await _gameService.GetAll(filter));

    [HttpGet(nameof(GetGameShortInfo))]
    public async Task<IActionResult> GetGameShortInfo([FromQuery] string name)
        => Ok(await Mediator.Send(new GetGameQuery(name)));

    [HttpGet(nameof(GetGameStatus))]
    public async Task<IActionResult> GetGameStatus([FromQuery] string name)
        => Ok(await _gameService.GameStatus(name));

    [HttpGet(nameof(GetGameConfigurations))]
    public async Task<IActionResult> GetGameConfigurations([FromQuery] ConfigurationsRequest request)
        => Ok(await _gameService.GetConfigurations(request.Name, request.PromotionId, request.ConfigurationId));

    [HttpGet(nameof(GetGameConfigurationMetadata))]
    public async Task<IActionResult> GetGameConfigurationMetadata([FromQuery] string name)
        => Ok(await Mediator.Send(new GetConfigurationMetadataQuery(name)));

    [HttpPost(nameof(ActivateGame))]
    public async Task<IActionResult> ActivateGame(string name)
        => Ok(await _gameService.ActivateGame(name));

    [HttpPost(nameof(DeactivateGame))]
    public async Task<IActionResult> DeactivateGame(string name)
        => Ok(await _gameService.DeactivateGame(name));

    [HttpPost(nameof(CreateConfiguration))]
    public async Task<IActionResult> CreateConfiguration([FromQuery] string gameName, [FromBody] object configurationJson)
        => Ok(await Mediator.Send(new CreateConfigurationCommand(gameName, configurationJson)));

    [HttpPut(nameof(UpdateConfiguration))]
    public async Task<IActionResult> UpdateConfiguration([FromQuery] string gameName, [FromBody] GameConfigurationDto configurationJson)
        => Ok(await Mediator.Send(new UpdateConfigurationCommand(gameName, configurationJson)));

    [HttpPatch(nameof(ActivateConfiguration))]
    public async Task<IActionResult> ActivateConfiguration([FromQuery] ConfigurationRequest request)
        => Ok(await Mediator.Send(new ActivateConfigurationCommand(request.Name, request.Id)));

    [HttpPatch(nameof(DeactivateConfiguration))]
    public async Task<IActionResult> DeactivateConfiguration([FromQuery] ConfigurationRequest request)
        => Ok(await Mediator.Send(new DeactivateConfigurationCommand(request.Name, request.Id)));

    #endregion

    #region Game Configuration Template

    [HttpPost(nameof(CreateGameConfigurationTemplate))]
    public async Task<ActionResult<GameConfigurationTemplate>> CreateGameConfigurationTemplate([FromQuery] string gameName, [FromBody] object command)
        => Ok(await Mediator.Send(new CreateGameConfigurationTemplateCommand(gameName, command)));

    [HttpDelete(nameof(DeleteGameConfigurationTemplate))]
    public async Task<IActionResult> DeleteGameConfigurationTemplate([FromQuery] DeleteGameConfigurationTemplateCommand command)
        => Ok(await Mediator.Send(command));

    [HttpGet(nameof(GetAllGameConfigurationTemplates))]
    public async Task<ActionResult<ApplicationResult<PaginatedResult<GameConfigurationTemplateDto>>>> GetAllGameConfigurationTemplates([FromQuery] GameTemplateFilter filter)
        => Ok(await Mediator.Send(new GetAllGameConfigurationTemplatesQuery(filter)));

    [HttpGet(nameof(GetGameConfigurationTemplateById))]
    public async Task<ActionResult<ApplicationResult<GameConfigurationTemplate>>> GetGameConfigurationTemplateById([FromQuery] string id)
        => Ok(await _gameTemplateService.GetGameConfigurationTemplateById(id));

    #endregion
}