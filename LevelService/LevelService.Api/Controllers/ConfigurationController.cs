using LevelService.Application.Features.ConfigurationFeatures.Commands.Create;
using LevelService.Application.Features.ConfigurationFeatures.Commands.Delete;
using LevelService.Application.Features.ConfigurationFeatures.Commands.Update;
using LevelService.Application.Features.ConfigurationFeatures.Queries.Get;
using Microsoft.AspNetCore.Mvc;

namespace LevelService.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ConfigurationController : BaseApiController
{
    public ConfigurationController()
    {
    }

    [HttpPost(nameof(CreateConfigurations))]
    public async Task CreateConfigurations([FromBody] CreateConfigurationCommand request) => await Mediator.Send(request);

    [HttpPut(nameof(UpdateConfiguration))]
    public async Task UpdateConfiguration([FromBody] UpdateConfigurationCommand request) => await Mediator.Send(request);

    [HttpPut(nameof(DeleteConfiguration))]
    public async Task DeleteConfiguration([FromQuery] DeleteConfigurationCommand request) => await Mediator.Send(request);

    [HttpGet(nameof(GetConfigurations))]
    public async Task<GetConfigurationsQueryResponse> GetConfigurations([FromQuery] GetConfigurationsQuery request) => await Mediator.Send(request);
}
