﻿using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.API.Controllers.Abstract;
using OnAim.Admin.APP.Features.GameFeatures.Queries.GetAllGames;
using OnAim.Admin.APP.Features.GameFeatures.Queries.GetById;
using OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetGameConfigurations.GetConfiguration;

namespace OnAim.Admin.API.Controllers;

public class GamesController : ApiControllerBase
{
    [HttpGet(nameof(GetAll))]
    public async Task<IActionResult> GetAll()
        => Ok(await Mediator.Send(new GetAllActiveGamesQuery()));

    [HttpGet(nameof(GetById) + "/id")]
    public async Task<IActionResult> GetById([FromRoute] int id)
        => Ok(await Mediator.Send(new GetGameQuery(id)));

    [HttpGet(nameof(GetConfiguration) + "/id")]
    public async Task<IActionResult> GetConfiguration([FromRoute] int id)
        => Ok(await Mediator.Send(new GetConfigurationQuery(id)));
}