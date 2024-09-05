using Hub.Application.Features.PlayerFeatures.Queries.GetPlayerBalance;
using Hub.Application.Features.PlayerFeatures.Queries.GetPlayer;
using Hub.Application.Features.PlayerFeatures.Queries.GetPlayers;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Hub.Application.Features.PlayerFeatures.Queries.GetPlayerProgress;

namespace Hub.Api.Controllers;

public class PlayerController : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<GetPlayersResponse>> GetAllAsync([FromQuery] int? page, int? pageSize)
    {
        var query = new GetPlayersQuery
        {
            PageNumber = page,
            PageSize = pageSize,
        };

        return Ok(await Mediator.Send(query));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetPlayerResponse>> GetAsync(int id)
    {
        return Ok(await Mediator.Send(new GetPlayerQuery { PlayerId = id }));
    }

    [HttpGet("balances")]
    public async Task<ActionResult<GetPlayerBalanceResponse>> GetPlayerBalancesAsync()
    {
        return Ok(await Mediator.Send(new GetPlayerBalanceQuery()));
    }

    [HttpGet("progress")]
    public async Task<ActionResult<GetPlayerProgressResponse>> GetPlayerProgressAsync()
    {
        return Ok(await Mediator.Send(new GetPlayerProgressQuery()));
    }

    //[HttpGet]
    //public async Task<Response<GetGamesBySegmentIdQueryResponse>> GetGames(GetGamesBySegmentIdQuery request) => await Mediator.Send(request);
}