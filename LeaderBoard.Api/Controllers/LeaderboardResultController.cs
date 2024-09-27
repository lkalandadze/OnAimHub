using Leaderboard.Application.Features.LeaderboardResultFeatures.Queries.Get;
using Leaderboard.Application.Features.LeaderboardResultFeatures.Queries.GetByPlayerId;
using Microsoft.AspNetCore.Mvc;

namespace Leaderboard.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class LeaderboardResultController : BaseApiController
{
    [HttpGet(nameof(GetLeaderboardResults))]
    public async Task<ActionResult<GetLeaderboardResultQueryResponse>> GetLeaderboardResults([FromQuery] GetLeaderboardResultQuery request) => await Mediator.Send(request);

    [HttpGet(nameof(GetLeaderboardByPlayerIdResults))]
    public async Task<ActionResult<GetLeaderboardResultsByPlayerIdQueryResponse>> GetLeaderboardByPlayerIdResults([FromQuery] GetLeaderboardResultsByPlayerIdQuery request) => await Mediator.Send(request);
}
