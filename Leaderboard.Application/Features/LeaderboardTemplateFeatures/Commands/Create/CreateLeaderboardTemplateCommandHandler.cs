using Leaderboard.Domain.Abstractions.Repository;
using Leaderboard.Domain.Entities;
using MediatR;

namespace Leaderboard.Application.Features.LeaderboardTemplateFeatures.Commands.Create;

public class CreateLeaderboardTemplateCommandHandler : IRequestHandler<CreateLeaderboardTemplateCommand>
{
    private readonly ILeaderboardTemplateRepository _leaderboardTemplateRepository;
    public CreateLeaderboardTemplateCommandHandler(ILeaderboardTemplateRepository leaderboardTemplateRepository)
    {
        _leaderboardTemplateRepository = leaderboardTemplateRepository;
    }

    public async Task Handle(CreateLeaderboardTemplateCommand request, CancellationToken cancellationToken)
    {
        var leaderboard = new LeaderboardTemplate(
            request.Name,
            request.JobType,
            request.StartTime,
            request.DurationInDays,
            request.AnnouncementLeadTimeInDays,
            request.CreationLeadTimeInDays);

        foreach (var prize in request.LeaderboardPrizes)
        {
            leaderboard.AddLeaderboardTemplatePrizes(prize.StartRank, prize.EndRank, prize.PrizeId, prize.Amount);
        }

        await _leaderboardTemplateRepository.InsertAsync(leaderboard);
        await _leaderboardTemplateRepository.SaveChangesAsync(cancellationToken);
    }
}