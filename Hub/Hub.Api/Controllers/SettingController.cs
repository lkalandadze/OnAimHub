using Hub.Application.Features.SettingFeatures.Commands.Update;
using Hub.Application.Features.SettingFeatures.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers;

[AllowAnonymous]
public class SettingController : BaseApiController
{
    [HttpGet(nameof(GetSettings))]
    public async Task<ActionResult<GetSettingsQueryResponse>> GetSettings([FromQuery] GetSettingsQuery request) => await Mediator.Send(request);

    [HttpPut(nameof(UpdateSettings))]
    public async Task<Unit> UpdateSettings(UpdateSettingCommand request) => await Mediator.Send(request);
}
