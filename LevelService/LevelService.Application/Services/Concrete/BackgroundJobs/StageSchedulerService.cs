using Hangfire;
using LevelService.Application.Services.Abstract.BackgroundJobs;
using LevelService.Domain.Abstractions.Repository;
using LevelService.Domain.Entities;
using LevelService.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace LevelService.Application.Services.Concrete.BackgroundJobs;

public class StageSchedulerService : IStageSchedulerService
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IStageRepository _stageRepository;
    public StageSchedulerService(
        IBackgroundJobClient backgroundJobClient,
        IRecurringJobManager recurringJobManager,
        IStageRepository stageRepository)
    {
        _backgroundJobClient = backgroundJobClient;
        _recurringJobManager = recurringJobManager;
        _stageRepository = stageRepository;
    }

    public void ScheduleActJobs(Stage stage)
    {
        if (stage.IsExpirable)
        {
            _backgroundJobClient.Schedule(() => StartAct(stage.Id), stage.DateFrom.Value.UtcDateTime);

            _backgroundJobClient.Schedule(() => FinishAct(stage.Id), stage.DateTo.Value.UtcDateTime);
        }
    }

    public async Task StartAct(int actId)
    {
        var act = await _stageRepository.Query().FirstOrDefaultAsync(x => x.Id == actId);
        if (act == default)
            throw new Exception("Stage not found");

        var inProgressActs = _stageRepository.Query().Where(x => x.Status == StageStatus.InProgress);
        foreach (var inProgressAct in inProgressActs)
        {
            inProgressAct.UpdateStatus(StageStatus.Finished);
            _stageRepository.Update(inProgressAct);
        }


        act.UpdateStatus(StageStatus.InProgress);
        _stageRepository.Update(act);
        await _stageRepository.SaveChangesAsync();
    }

    public async Task FinishAct(int actId)
    {
        var act = await _stageRepository.Query().FirstOrDefaultAsync(x => x.Id == actId);
        if (act == default)
            throw new Exception("Stage not found");

        act.UpdateStatus(StageStatus.Finished);
        _stageRepository.Update(act);
        await _stageRepository.SaveChangesAsync();
    }
}
