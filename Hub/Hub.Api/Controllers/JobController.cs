using Hub.Application.Models.Job;
using Hub.Application.Services.Abstract.BackgroundJobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api.Controllers;

[AllowAnonymous]
public class JobController : BaseApiController
{
    private readonly IJobService _jobService;

    public JobController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobModel job)
    {
        await _jobService.CreateJobAsync(job);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetJobs()
    {
        var jobs = await _jobService.GetAllJobsAsync();
        return Ok(jobs);
    }

    [HttpPost(nameof(SyncJobs))]
    public async Task<IActionResult> SyncJobs()
    {
        await _jobService.SyncJobsWithHangfireAsync();
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteJob([FromQuery] int id)
    {
        await _jobService.DeleteJobAsync(id);
        return Ok();
    }
}
