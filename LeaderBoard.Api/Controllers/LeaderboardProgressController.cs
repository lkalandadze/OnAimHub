using Leaderboard.Application.Features.LeaderboardProgressFeatures.Commands.Upsert;
using Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.Get;
using Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.GetForUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Leaderboard.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class LeaderboardProgressController : BaseApiController
{
    [Authorize]
    [HttpPost(nameof(UpsertProgress))]
    public async Task UpsertProgress(UpsertProgressCommand request) => await Mediator.Send(request);

    [HttpGet(nameof(GetLeaderboardProgress))]
    public async Task<ActionResult<GetLeaderboardProgressQueryResponse>> GetLeaderboardProgress([FromQuery] GetLeaderboardProgressQuery request) => await Mediator.Send(request);

    [HttpGet(nameof(GetLeaderboardProgressForUser))]
    public async Task<ActionResult<GetLeaderboardProgressForUserQueryResponse>> GetLeaderboardProgressForUser([FromQuery] GetLeaderboardProgressForUserQuery request) => await Mediator.Send(request);
}
