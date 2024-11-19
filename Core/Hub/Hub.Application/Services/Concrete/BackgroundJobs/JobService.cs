using Hub.Application.Models.Job;
using Hub.Application.Services.Abstract.BackgroundJobs;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Enum;

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
            {
                string cronExpression = DetermineCronExpression(job);

                _jobScheduler.ScheduleJob(job, cronExpression);
            }
        }
    }

    public async Task<List<Job>> GetAllJobsAsync() => await _jobRepository.QueryAsync();

    public async Task CreateJobAsync(CreateJobModel request)
    {
        if (request.JobCategory == JobCategory.DailyProgressReset)
        {
            var jobs = _jobRepository.Query(j => j.Category == JobCategory.DailyProgressReset);

            if (jobs.Any())
            {
                throw new ArgumentException("A job with the category 'DailyProgressReset' already exists.");
            }
        }

        var job = new Job(request.Name, request.Description, request.IsActive, request.JobType, request.JobCategory, request.ExecutionTime, request.CurrencyId, request.IntervalInDays);

        await _jobRepository.InsertAsync(job);
        await _unitOfWork.SaveAsync();

        if (job.IsActive)
        {
            string cronExpression = DetermineCronExpression(job);

            _jobScheduler.ScheduleJob(job, cronExpression);
        }
    }

    public async Task DeleteJobAsync(int jobId)
    {
        var job = _jobRepository.Query().Where(x => x.Id == jobId).FirstOrDefault();

        if (job == default)
        {
            throw new InvalidOperationException($"Job with ID {jobId} not found.");
        }

        _jobScheduler.RemoveScheduledJob(job.Id);

        _jobRepository.Delete(job);
        await _unitOfWork.SaveAsync();
    }

    private string DetermineCronExpression(Job job)
    {
        switch (job.Type)
        {
            case JobType.Daily:
                return "0 0 0 * * ?"; // Every day at 12 AM

            case JobType.Weekly:
                return "0 0 0 * * 1"; // Every week on Monday at 12 AM

            case JobType.Monthly:
                return "0 0 0 1 * ?"; // Every month on the 1st at 12 AM

            case JobType.Custom:
                TimeSpan executionTime = job.ExecutionTime ?? TimeSpan.Zero;
                int intervalInDays = job.IntervalInDays ?? 1;

                return GenerateCustomCronExpression(executionTime, intervalInDays);

            default:
                throw new InvalidOperationException("Unsupported job type");
            }
    }

    private string GenerateCustomCronExpression(TimeSpan executionTime, int intervalInDays)
    {
        int hour = executionTime.Hours;
        int minute = executionTime.Minutes;

        return $"0 {minute} {hour} */{intervalInDays} * ?";
    }
}