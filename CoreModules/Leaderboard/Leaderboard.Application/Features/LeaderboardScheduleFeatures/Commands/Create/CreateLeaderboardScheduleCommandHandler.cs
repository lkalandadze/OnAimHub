using Leaderboard.Application.Services.Abstract.BackgroundJobs;
using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Application.Features.LeaderboardScheduleFeatures.Commands.Create;

public class CreateLeaderboardScheduleCommandHandler : IRequestHandler<CreateLeaderboardScheduleCommand>
{
    private readonly ILeaderboardScheduleRepository _leaderboardScheduleRepository;
    private readonly IBackgroundJobScheduler _backgroundJobScheduler;
    public CreateLeaderboardScheduleCommandHandler(ILeaderboardScheduleRepository leaderboardScheduleRepository, IBackgroundJobScheduler backgroundJobScheduler)
    {
        _leaderboardScheduleRepository = leaderboardScheduleRepository;
        _backgroundJobScheduler = backgroundJobScheduler;
    }

    public async Task Handle(CreateLeaderboardScheduleCommand request, CancellationToken cancellationToken)
    {
        //var template = await _leaderboardTemplateRepository.Query().FirstOrDefaultAsync(x => x.Id == request.LeaderboardTemplateId);

        //if (template == default)
        //    throw new Exception("Leaderboard template not found");

        //var leaderboardSchedule = new LeaderboardSchedule(
        //    template.Name,
        //    request.RepeatType,
        //    request.RepeatValue,
        //    request.StartDate,
        //    request.EndDate,
        //    request.LeaderboardTemplateId);

        //await _leaderboardScheduleRepository.InsertAsync(leaderboardSchedule);
        //await _leaderboardScheduleRepository.SaveChangesAsync(cancellationToken);

        //_backgroundJobScheduler.ScheduleJob(leaderboardSchedule);
    }
}