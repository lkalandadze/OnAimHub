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
    private readonly IPromotionService _promotionService;

    public HangfireJobScheduler(IRecurringJobManager recurringJobManager, IJobRepository jobRepository, IPlayerBalanceService playerBalanceService, IPlayerProgressService playerProgressService, IUnitOfWork unitOfWork, IPromotionService promotionService)
    {
        _recurringJobManager = recurringJobManager;
        _jobRepository = jobRepository;
        _playerBalanceService = playerBalanceService;
        _playerProgressService = playerProgressService;
        _unitOfWork = unitOfWork;
        _promotionService = promotionService;
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
                case JobCategory.CoinBalanceReset:
                    if (!string.IsNullOrEmpty(currentJob.CoinId))
                    {
                        await ResetBalancesByCoinIdAsync(currentJob);
                    }
                    break;
                case JobCategory.DailyProgressReset:
                    await ResetProgressesDailyAsync(currentJob);
                    break;

                case JobCategory.PromotionStatusUpdate:
                    await HandlePromotionStatusUpdateAsync(currentJob);
                    break;
            }
        }
    }

    private async Task ResetBalancesByCoinIdAsync(Job job)
    {
        await _playerBalanceService.ResetBalancesByCoinIdAsync(job.CoinId);

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

    public async Task HandlePromotionStatusUpdateAsync(Job job)
    {
        var promotionId = ExtractPromotionIdFromJobName(job.Name);

        if (job.Name.Contains("Start"))
        {
            await _promotionService.UpdatePromotionStatus(promotionId, PromotionStatus.Started);
        }
        else if (job.Name.Contains("Finish"))
        {
            await _promotionService.UpdatePromotionStatus(promotionId, PromotionStatus.Finished);
        }

        job.SetLastExecutedTime();
        _jobRepository.Update(job);
        await _unitOfWork.SaveAsync();
    }

    private int ExtractPromotionIdFromJobName(string jobName)
    {
        var parts = jobName.Split('-');
        return int.Parse(parts[1]); // Assuming "Promotion-{Id}-Start"
    }
}