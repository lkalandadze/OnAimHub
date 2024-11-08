using Hub.Domain.Entities;
using Hub.Domain.Enum;

namespace Hub.Application.Services.Abstract.BackgroundJobs;

public interface IActSchedulerService
{
    void ScheduleActJobs(Act act);

    Task StartAct(int actId);

    Task FinishAct(int actId);
}
