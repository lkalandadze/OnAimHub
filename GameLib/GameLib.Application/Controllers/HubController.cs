using GameLib.Application.Models.Configuration;
using GameLib.Application.Models.Game;
using GameLib.Application.Services.Abstract;
using GameLib.Application.Services.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameLib.Application.Controllers;

public class HubController : BaseApiController
{
    private readonly IGameInfoService _gameInfoService;
    private readonly IConfigurationService _configurationService;

    public HubController(IConfigurationService configurationService, IGameInfoService gameInfoService)
    {
        _configurationService = configurationService;
        _gameInfoService = gameInfoService;
    }

    [HttpGet(nameof(Configurations))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<ConfigurationBaseGetModel>>> Configurations()
    {
        return Ok(await _configurationService.GetAllAsync());
    }

    [HttpGet("Configurations/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ConfigurationBaseGetModel>> GetConfigurationByIdAsync([FromRoute] int id)
    {
        return Ok(await _configurationService.GetConfigurationByIdAsync(id));
    }

    [HttpGet(nameof(GameShortInfo))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<GetGameShortInfoModel>>> GameShortInfo()
    {
        return Ok(await _gameInfoService.GetGameShortInfo());
    }
}