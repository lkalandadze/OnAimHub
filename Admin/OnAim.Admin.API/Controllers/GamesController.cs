using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.GameFeatures.Commands.ActivateConfiguration;
using OnAim.Admin.APP.Features.GameFeatures.Commands.CreateConfiguration;
using OnAim.Admin.APP.Features.GameFeatures.Commands.CreatePrizeType;
using OnAim.Admin.APP.Features.GameFeatures.Commands.DeactivateConfiguration;
using OnAim.Admin.APP.Features.GameFeatures.Commands.UpdateConfiguration;
using OnAim.Admin.APP.Features.GameFeatures.Commands.UpdatePrizeType;
using OnAim.Admin.APP.Features.GameFeatures.Queries.GetAllGames;
using OnAim.Admin.APP.Features.GameFeatures.Queries.GetById;
using OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetConfigurationMetadata;
using OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetGameConfigurations;
using OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetGameConfigurations.GetConfiguration;
using OnAim.Admin.APP.Features.GameFeatures.Queries.GetPrizeTypeById;
using OnAim.Admin.APP.Features.GameFeatures.Queries.GetPrizeTypes;
using OnAim.Admin.APP.Features.GameFeatures.Template.Commands.Create;
using OnAim.Admin.APP.Features.GameFeatures.Template.Commands.Delete;
using OnAim.Admin.APP.Features.GameFeatures.Template.Queries.GetAll;
using OnAim.Admin.APP.Services.Game;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Dtos.Game;

namespace OnAim.Admin.API.Controllers;

public class GamesController : ApiControllerBase
{
    [HttpGet(nameof(GetAll))]
    public async Task<IActionResult> GetAll()
        => Ok(await Mediator.Send(new GetAllActiveGamesQuery()));

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

    [HttpGet(nameof(GetPrizeTypes))]
    public async Task<IActionResult> GetPrizeTypes()
        => Ok(await Mediator.Send(new GetPrizeTypesQuery()));    

    [HttpGet(nameof(GetPrizeTypeById))]
    public async Task<IActionResult> GetPrizeTypeById([FromQuery] int id)
        => Ok(await Mediator.Send(new GetPrizeTypeByIdQuery(id)));

    [HttpPost(nameof(CreateConfiguration))]
    public async Task<IActionResult> CreateConfiguration([FromBody] ConfigurationRequest configurationJson)
        => Ok(await Mediator.Send(new CreateConfigurationCommand(configurationJson.ConfigurationJson)));

    [HttpPut(nameof(UpdateConfiguration))]
    public async Task<IActionResult> UpdateConfiguration([FromBody] ConfigurationRequest configurationJson)
        => Ok(await Mediator.Send(new UpdateConfigurationCommand(configurationJson.ConfigurationJson)));

    [HttpPut(nameof(ActivateConfiguration))]
    public async Task<IActionResult> ActivateConfiguration([FromQuery] int id)
        => Ok(await Mediator.Send(new ActivateConfigurationCommand(id)));

    [HttpPut(nameof(DeactivateConfiguration))]
    public async Task<IActionResult> DeactivateConfiguration([FromQuery] int id)
        => Ok(await Mediator.Send(new DeactivateConfigurationCommand(id)));

    [HttpPost(nameof(CreatePrizeType))]
    public async Task<IActionResult> CreatePrizeType([FromBody] CreatePrizeTypeDto createPrizeType)
        => Ok(await Mediator.Send(new CreatePrizeTypeCommand(createPrizeType)));

    [HttpPut(nameof(UpdatePrizeType))]
    public async Task<IActionResult> UpdatePrizeType([FromQuery] int id,[FromBody] CreatePrizeTypeDto createPrizeType)
    => Ok(await Mediator.Send(new UpdatePrizeTypeCommand(id,createPrizeType)));

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
}
