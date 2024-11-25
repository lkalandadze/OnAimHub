﻿using Leaderboard.Application.Services.Abstract.BackgroundJobs;
using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using MediatR;

namespace Leaderboard.Application.Features.LeaderboardTemplateFeatures.Commands.Create;

public class CreateLeaderboardTemplateCommandHandler : IRequestHandler<CreateLeaderboardTemplateCommand>
{
    private readonly ILeaderboardTemplateRepository _leaderboardTemplateRepository;
    private readonly IBackgroundJobScheduler _backgroundJobScheduler;
    private readonly IJobService _jobService;
    public CreateLeaderboardTemplateCommandHandler(ILeaderboardTemplateRepository leaderboardTemplateRepository, IBackgroundJobScheduler backgroundJobScheduler, IJobService jobService)
    {
        _leaderboardTemplateRepository = leaderboardTemplateRepository;
        _backgroundJobScheduler = backgroundJobScheduler;
        _jobService = jobService;
    }

    public async Task Handle(CreateLeaderboardTemplateCommand request, CancellationToken cancellationToken)
    {
        var leaderboardTemplate = new LeaderboardTemplate(
            request.Name,
            request.Description,
            request.StartTime,
            request.EndIn,
            request.StartIn,
            request.AnnounceIn,
            request.CorrelationId);

        foreach (var prize in request.LeaderboardPrizes)
        {
            leaderboardTemplate.AddLeaderboardTemplatePrizes(prize.StartRank, prize.EndRank, prize.PrizeId, prize.Amount);
        }

        await _leaderboardTemplateRepository.InsertAsync(leaderboardTemplate);
        await _leaderboardTemplateRepository.SaveChangesAsync(cancellationToken);
    }
}