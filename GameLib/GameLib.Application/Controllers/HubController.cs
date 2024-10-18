using GameLib.Application.Models.Configuration;
using GameLib.Application.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameLib.Application.Controllers;

public class HubController : BaseApiController
{
    private readonly IConfigurationService _configurationService;

    public HubController(IConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    [HttpGet("Configurations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<ConfigurationBaseGetModel>>> GetConfigurationsAsync()
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
}