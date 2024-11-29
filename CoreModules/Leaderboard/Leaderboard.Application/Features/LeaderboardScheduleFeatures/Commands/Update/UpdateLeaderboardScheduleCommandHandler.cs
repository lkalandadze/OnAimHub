using Leaderboard.Application.Services.Abstract.BackgroundJobs;
using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Application.Features.LeaderboardScheduleFeatures.Commands.Update;

public class UpdateLeaderboardScheduleCommandHandler : IRequestHandler<UpdateLeaderboardScheduleCommand>
{
    private readonly ILeaderboardScheduleRepository _leaderboardScheduleRepository;
    private readonly IBackgroundJobScheduler _backgroundJobScheduler;

    public UpdateLeaderboardScheduleCommandHandler(ILeaderboardScheduleRepository leaderboardScheduleRepository, IBackgroundJobScheduler backgroundJobScheduler)
    {
        _leaderboardScheduleRepository = leaderboardScheduleRepository;
        _backgroundJobScheduler = backgroundJobScheduler;
    }

    public async Task Handle(UpdateLeaderboardScheduleCommand request, CancellationToken cancellationToken)
    {
        var leaderboardSchedule = await _leaderboardScheduleRepository.Query().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (leaderboardSchedule == default)
            throw new Exception("Leaderboard schedule not found");

        //leaderboardSchedule.UpdateStatus(request.Status);

        if (request.Status == LeaderboardScheduleStatus.Cancelled)
            _backgroundJobScheduler.RemoveScheduledJob(leaderboardSchedule.Id);

        _leaderboardScheduleRepository.Update(leaderboardSchedule);
        await _leaderboardScheduleRepository.SaveChangesAsync(cancellationToken);
    }
}