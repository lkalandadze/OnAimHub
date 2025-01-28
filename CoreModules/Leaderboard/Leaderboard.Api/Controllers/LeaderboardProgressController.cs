using Leaderboard.Application.Features.LeaderboardProgressFeatures.Commands.Upsert;
using Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.Admin.GetPlayerActiveLeaderboards;
using Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.User.Get;
using Leaderboard.Application.Features.LeaderboardProgressFeatures.Queries.User.GetForUser;
using Leaderboard.Application.Features.LeaderboardScheduleFeatures.Commands.Test;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Leaderboard.Api.Controllers;

[Route("LeaderboardApi/[controller]")]
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

    [HttpGet(nameof(GetUserActiveLeaderboards))]
    public async Task<ActionResult<GetPlayerActiveLeaderboardsQueryResponse>> GetUserActiveLeaderboards([FromQuery] GetPlayerActiveLeaderboardsQuery request) => await Mediator.Send(request);

    [HttpPost(nameof(FinishLeaderboard))]
    public async Task FinishLeaderboard([FromQuery] FinishLeaderboardCommand request) => await Mediator.Send(request);
}
