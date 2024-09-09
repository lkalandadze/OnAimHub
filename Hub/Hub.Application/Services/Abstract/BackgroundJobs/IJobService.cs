using Hub.Application.Models.Job;
using Hub.Domain.Entities;

namespace Hub.Application.Services.Abstract.BackgroundJobs;

public interface IJobService
{
    Task SyncJobsWithHangfireAsync();
    Task<List<Job>> GetAllJobsAsync();
    Task CreateJobAsync(CreateJobModel job);
}
