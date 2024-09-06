using Hub.Application.Models.Job;
using Hub.Application.Services.Abstract.BackgroundJobs;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;

namespace Hub.Application.Services.Concrete.BackgroundJobs;

public class JobService : IJobService
{
    private readonly IJobRepository _jobRepository;
    private readonly IBackgroundJobScheduler _jobScheduler;
    private readonly IUnitOfWork _unitOfWork;

    public JobService(IJobRepository jobRepository, IBackgroundJobScheduler jobScheduler, IUnitOfWork unitOfWork)
    {
        _jobRepository = jobRepository;
        _jobScheduler = jobScheduler;
        _unitOfWork = unitOfWork;
    }

    public async Task SyncJobsWithHangfireAsync()
    {
        var jobs = await _jobRepository.QueryAsync();

        foreach (var job in jobs)
        {
            if (job.IsActive)
                _jobScheduler.ScheduleJob(job);
        }
    }


    public async Task<List<Job>> GetAllJobsAsync() => await _jobRepository.QueryAsync();

    public async Task CreateJobAsync(CreateJobModel request)
    {
        var job = new Job(request.Name, request.Description, request.CurrencyId, request.CronExpression, request.IsActive);

        await _jobRepository.InsertAsync(job);
        await _unitOfWork.SaveAsync();

        if (job.IsActive)
            _jobScheduler.ScheduleJob(job);
    }
}
