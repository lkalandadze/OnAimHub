using LevelService.Domain.Entities;

namespace LevelService.Application.Services.Abstract.BackgroundJobs;

public interface IStageSchedulerService
{
    void ScheduleActJobs(Stage stage);

    Task StartAct(int stageId);

    Task FinishAct(int stageId);
}
