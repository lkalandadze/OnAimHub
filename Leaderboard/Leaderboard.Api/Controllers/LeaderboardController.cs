using Leaderboard.Application.Features.LeaderboardRecordFeatures.Commands.Create;
using Leaderboard.Application.Features.LeaderboardRecordFeatures.Commands.Update;
using Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.Get;
using Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.GetById;
using Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.GetCalendar;
using Leaderboard.Application.Features.LeaderboardTemplateFeatures.Commands.Create;
using Leaderboard.Application.Features.LeaderboardTemplateFeatures.Commands.Update;
using Leaderboard.Application.Features.LeaderboardTemplateFeatures.Queries.Get;
using Leaderboard.Application.Features.LeaderboardTemplateFeatures.Queries.GetById;
using Leaderboard.Application.Services.Abstract.BackgroundJobs;
using Microsoft.AspNetCore.Mvc;

namespace Leaderboard.Api.Controllers;

[Route("api/v1/[controller]")]
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

    [HttpPut(nameof(UpdateLeaderboardRecord))]
    public async Task UpdateLeaderboardRecord(UpdateLeaderboardRecordCommand request) => await Mediator.Send(request);

    [HttpGet(nameof(GetLeaderboardRecords))]
    public async Task<ActionResult<GetLeaderboardRecordsQueryResponse>> GetLeaderboardRecords([FromQuery] GetLeaderboardRecordsQuery request) => await Mediator.Send(request);

    [HttpGet(nameof(GetLeaderboardRecordById))]
    public async Task<ActionResult<GetLeaderboardRecordByIdQueryResponse>> GetLeaderboardRecordById([FromQuery] GetLeaderboardRecordByIdQuery request) => await Mediator.Send(request);

    #endregion



    #region LeaderboardTemplate

    [HttpPost(nameof(CreateLeaderboardTemplate))]
    public async Task CreateLeaderboardTemplate(CreateLeaderboardTemplateCommand request) => await Mediator.Send(request);

    [HttpPut(nameof(UpdateLeaderboardTemplates))]
    public async Task UpdateLeaderboardTemplates(UpdateLeaderboardTemplateCommand request) => await Mediator.Send(request);

    [HttpGet(nameof(GetLeaderboardTemplates))]
    public async Task<ActionResult<GetLeaderboardTemplatesQueryResponse>> GetLeaderboardTemplates([FromQuery] GetLeaderboardTemplatesQuery request) => await Mediator.Send(request);

    [HttpGet(nameof(GetCalendar))]
    public async Task<ActionResult<GetCalendarQueryResponse>> GetCalendar([FromQuery] GetCalendarQuery request) => await Mediator.Send(request);

    [HttpGet(nameof(GetLeaderboardTemplateById))]
    public async Task<ActionResult<GetLeaderboardTemplateByIdQueryResponse>> GetLeaderboardTemplateById([FromQuery] GetLeaderboardTemplateByIdQuery request) => await Mediator.Send(request);

    #endregion Test

    [HttpPost("schedule/{templateId}")]
    public async Task<IActionResult> ScheduleJob(int templateId)
    {
        try
        {
            var jobs = await _jobService.GetAllJobsAsync();
            var job = jobs.FirstOrDefault(j => j.LeaderboardTemplateId == templateId);

            if (job == null)
            {
                return NotFound($"Leaderboard job for template ID {templateId} not found.");
            }

            // Schedule the job with the template ID
            _backgroundJobScheduler.ScheduleJob(job.LeaderboardTemplate);
            return Ok($"Job for template ID {templateId} has been scheduled.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error scheduling job: {ex.Message}");
        }
    }

    [HttpPost("execute/{templateId}")]
    public async Task<IActionResult> ExecuteJob(int templateId)
    {
        try
        {
            await _jobService.ExecuteLeaderboardRecordGeneration(templateId);
            return Ok($"Leaderboard record generation for template ID {templateId} has been executed.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error executing job: {ex.Message}");
        }
    }
}
