﻿using Hub.Application.Features.GameFeatures.Queries.GetAllGame;
using Hub.Application.Models.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers;

[AllowAnonymous] //TEMP
public class HubController : BaseApiController
{
    [HttpGet("Games")]
    public async Task<ActionResult<IEnumerable<ActiveGameModel>>> GetAllGameAsync()
    {
        return Ok(await Mediator.Send(new GetAllGameQuery()));
    }
}