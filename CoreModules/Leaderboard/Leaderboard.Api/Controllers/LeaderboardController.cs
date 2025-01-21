using Leaderboard.Application.Features.LeaderboardRecordFeatures.Commands.Create;
using Leaderboard.Application.Features.LeaderboardRecordFeatures.Commands.Delete;
using Leaderboard.Application.Features.LeaderboardRecordFeatures.Commands.Update;
using Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.Get;
using Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.GetById;
using Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.GetCalendar;
using Leaderboard.Application.Features.LeaderboardScheduleFeatures.Commands.Create;
using Leaderboard.Application.Features.LeaderboardScheduleFeatures.Commands.Update;
using Leaderboard.Application.Features.LeaderboardScheduleFeatures.Queries.Get;
using Leaderboard.Application.Services.Abstract.BackgroundJobs;
using Microsoft.AspNetCore.Mvc;

namespace Leaderboard.Api.Controllers;

[Route("LeaderboardApi/[controller]")]
[ApiController]
public class LeaderboardController : BaseApiController
{
    private readonly IJobService _jobService;
    private readonly IBackgroundJobScheduler _backgroundJobScheduler;
    public LeaderboardController(IJobService jobService, IBackgroundJobScheduler backgroundJobScheduler)
    {
        _jobService = jobService;
        _backgroundJobScheduler = backgroundJobScheduler;
    }

    #region LeaderboardRecord

    [HttpPost(nameof(CreateLeaderboardRecord))]
    public async Task CreateLeaderboardRecord(CreateLeaderboardRecordCommand request) => await Mediator.Send(request);

    [HttpPost(nameof(DeleteLeaderboardRecord))]
    public async Task DeleteLeaderboardRecord(DeleteLeaderboardRecordCommand request) => await Mediator.Send(request);

    [HttpPut(nameof(UpdateLeaderboardRecord))]
    public async Task UpdateLeaderboardRecord(UpdateLeaderboardRecordCommand request) => await Mediator.Send(request);

    //[Authorize]
    [HttpGet(nameof(GetLeaderboardRecords))]
    public async Task<ActionResult<GetLeaderboardRecordsQueryResponse>> GetLeaderboardRecords([FromQuery] GetLeaderboardRecordsQuery request) => await Mediator.Send(request);

    [HttpGet(nameof(GetLeaderboardRecordById))]
    public async Task<ActionResult<GetLeaderboardRecordByIdQueryResponse>> GetLeaderboardRecordById([FromQuery] GetLeaderboardRecordByIdQuery request) => await Mediator.Send(request);

    #endregion


    #region LeaderboardSchedule

    [HttpPost(nameof(CreateLeaderboardSchedule))]
    public async Task CreateLeaderboardSchedule([FromBody] CreateLeaderboardScheduleCommand request) => await Mediator.Send(request);

    [HttpPut(nameof(UpdateLeaderboardSchedule))]
    public async Task UpdateLeaderboardSchedule([FromBody] UpdateLeaderboardScheduleCommand request) => await Mediator.Send(request);

    [HttpGet(nameof(GetLeaderboardSchedules))]
    public async Task<ActionResult<GetLeaderboardSchedulesQueryResponse>> GetLeaderboardSchedules([FromQuery] GetLeaderboardSchedulesQuery request) => await Mediator.Send(request);

    #endregion
    [HttpGet(nameof(GetCalendar))]
    public async Task<ActionResult<GetCalendarQueryResponse>> GetCalendar([FromQuery] GetCalendarQuery request) => await Mediator.Send(request);

}
