using GameLib.Application.Models.Configuration;
using GameLib.Application.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameLib.Application.Controllers;

public class ConfigurationController : BaseApiController
{
    private readonly IConfigurationService _configurationService;

    public ConfigurationController(IConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> CreateAsync([FromBody] ConfigurationCreateModel model)
    {
        await _configurationService.CreateAsync(model);
        return StatusCode(201);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] ConfigurationUpdateModel model)
    {
        await _configurationService.UpdateAsync(id, model);
        return StatusCode(200);
    }

    [HttpPatch("{id}/Activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> ActivateAsync([FromRoute] int id)
    {
        await _configurationService.ActivateAsync(id);
        return StatusCode(200);
    }

    [HttpPatch("{id}/Deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> DeactivateAsync([FromRoute] int id)
    {
        await _configurationService.DeactivateAsync(id);
        return StatusCode(200);
    }

    [HttpPost("{id}/AssignSegments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> AssignConfigurationToSegmentsAsync([FromRoute] int id, [FromBody] IEnumerable<string> segmentIds)
    {
        await _configurationService.AssignConfigurationToSegmentsAsync(id, segmentIds);
        return StatusCode(200);
    }

    [HttpPost("{id}/UnassignSegments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> UnassignConfigurationToSegmentsAsync([FromRoute] int id, [FromBody] IEnumerable<string> segmentIds)
    {
        await _configurationService.UnassignConfigurationToSegmentsAsync(id, segmentIds);
        return StatusCode(200);
    }
}