using GameLib.Application.Controllers;
using GameLib.Application.Generators;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Wheel.Application.Features.ConfigurationFeatures.Commands.Create;
using Wheel.Domain.Entities;

namespace Wheel.Api.Controllers;

public class ConfigurationController : BaseApiController
{
    private readonly CommandGenerator _commandGenerator;
    public ConfigurationController(CommandGenerator commandGenerator)
    {
        _commandGenerator = commandGenerator;
    }

    [HttpGet("test")]
    public IActionResult GetConfigurationMetadata()
    {
        var createCommand = _commandGenerator.GenerateCreateCommandWithDtos(typeof(WheelConfiguration));
        return Ok(createCommand);
    }


    //[HttpPost(nameof(CreateConfiguration))]
    //public async Task CreateConfiguration(CreateConfigurationCommand request) => await Mediator.Send(request);

    [HttpPost(nameof(CreateConfiguration))]
    public async Task<IActionResult> CreateConfiguration([FromBody] CreateConfigurationCommand configurationData)
    {

        await Mediator.Send(configurationData);
        return Ok();
    }
}
