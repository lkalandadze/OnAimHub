using Hangfire;
using Hub.Application.Services.Abstract;
using Hub.Application.Services.Abstract.BackgroundJobs;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Enum;

namespace Hub.Application.Services.Concrete.BackgroundJobs;

public class HangfireJobScheduler : IBackgroundJobScheduler
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IJobRepository _jobRepository;
    private readonly IPlayerBalanceService _playerBalanceService;
    private readonly IPlayerProgressService _playerProgressService;
    private readonly IUnitOfWork _unitOfWork;

    public HangfireJobScheduler(IRecurringJobManager recurringJobManager, IJobRepository jobRepository, IPlayerBalanceService playerBalanceService, IPlayerProgressService playerProgressService, IUnitOfWork unitOfWork)
    {
        _recurringJobManager = recurringJobManager;
        _jobRepository = jobRepository;
        _playerBalanceService = playerBalanceService;
        _playerProgressService = playerProgressService;
        _unitOfWork = unitOfWork;
    }

    public void ScheduleJob(Job job, string cronExpression)
    {
        RecurringJob.AddOrUpdate(job.Id.ToString(), () => ExecuteJobs(job), cronExpression);
    }

    public void RemoveScheduledJob(int jobId)
    {
        RecurringJob.RemoveIfExists(jobId.ToString());
    }

    public async Task ExecuteJobs(Job job)
    {
        var currentJob = _jobRepository.Query(x => x.Id == job.Id)
                                       .FirstOrDefault();

        if (currentJob != null)
        {
            switch (currentJob.Category)
            {
                case JobCategory.CurrencyBalanceReset:
                    if (!string.IsNullOrEmpty(currentJob.CurrencyId))
                    {
                        await ResetBalancesByCurrencyIdAsync(currentJob);
                    }
                    break;
                case JobCategory.DailyProgressReset:
                    await ResetProgressesDailyAsync(currentJob);
                    break;
            }
        }
    }

    private async Task ResetBalancesByCurrencyIdAsync(Job job)
    {
        await _playerBalanceService.ResetBalancesByCurrencyIdAsync(job.CurrencyId);

        job.SetLastExecutedTime();

        _jobRepository.Update(job);
        await _unitOfWork.SaveAsync();
    }

    private async Task ResetProgressesDailyAsync(Job job)
    {
        await _playerProgressService.ResetPlayerProgressesAndSaveHistoryAsync();

        job.SetLastExecutedTime();

        _jobRepository.Update(job);
        await _unitOfWork.SaveAsync();
    }
}