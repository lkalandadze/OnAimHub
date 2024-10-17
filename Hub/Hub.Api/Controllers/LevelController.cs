using Hub.Application.Features.LevelFeatures.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers;

public class LevelController : BaseApiController
{
    [HttpPost(nameof(CreateLevels))]
    public async Task<ActionResult<Unit>> CreateLevels([FromQuery] CreateLevelCommand request) => await Mediator.Send(request);
}
