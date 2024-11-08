using Hangfire;
using Hub.Application.Services.Abstract.BackgroundJobs;
using Hub.Domain.Absractions;
using Hub.Domain.Absractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Hub.Application.Services.Concrete.BackgroundJobs;

public class ActSchedulerService : IActSchedulerService
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IActRepository _actRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActSchedulerService(
        IBackgroundJobClient backgroundJobClient,
        IRecurringJobManager recurringJobManager,
        IActRepository actRepository,
        IUnitOfWork unitOfWork)
    {
        _backgroundJobClient = backgroundJobClient;
        _recurringJobManager = recurringJobManager;
        _actRepository = actRepository;
        _unitOfWork = unitOfWork;
    }

    public void ScheduleActJobs(Act act)
    {
        _backgroundJobClient.Schedule(() => StartAct(act.Id), act.DateFrom.Value.UtcDateTime);

        _backgroundJobClient.Schedule(() => FinishAct(act.Id), act.DateTo.Value.UtcDateTime);
    }

    public async Task StartAct(int actId)
    {
        var act = await _actRepository.Query().FirstOrDefaultAsync(x => x.Id == actId);
        if (act == default)
            throw new Exception("Act not found");

        var inProgressActs = _actRepository.Query().Where(x => x.Status == ActStatus.InProgress);
        foreach (var inProgressAct in inProgressActs)
        {
            inProgressAct.UpdateStatus(ActStatus.Finished);
            _actRepository.Update(inProgressAct);
        }

        
        act.UpdateStatus(ActStatus.InProgress);
        _actRepository.Update(act);
        await _unitOfWork.SaveAsync();
    }

    public async Task FinishAct(int actId)
    {
        var act = await _actRepository.Query().FirstOrDefaultAsync(x => x.Id == actId);
        if (act == default)
            throw new Exception("Act not found");

        act.UpdateStatus(ActStatus.Finished);
        _actRepository.Update(act);
        await _unitOfWork.SaveAsync();
    }
}
