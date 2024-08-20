using Hub.Application.Features.PlayerFeatures.Queries.GetBalance;
using Hub.Application.Features.PlayerFeatures.Queries.GetPlayer;
using Hub.Application.Features.PlayerFeatures.Queries.GetPlayers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers;

[Authorize]
[Route("hubapi/[controller]")]
public class PlayerController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<GetPlayersResponse>> GetAllAsync()
    {
        return Ok(await Mediator.Send(new GetPlayersRequest()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetPlayerResponse>> GetAsync(int id)
    {
        return Ok(await Mediator.Send(new GetPlayerRequest { PlayerId = id }));
    }

    [HttpGet("balances")]
    public async Task<ActionResult<GetPlayerResponse>> GetBalancesAsync()
    {
        return Ok(await Mediator.Send(new GetBalanceRequest()));
    }
}