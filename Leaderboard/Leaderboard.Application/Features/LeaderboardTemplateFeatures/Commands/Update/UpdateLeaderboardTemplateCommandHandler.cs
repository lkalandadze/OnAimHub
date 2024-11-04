using Leaderboard.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Application.Features.LeaderboardTemplateFeatures.Commands.Update;

public class UpdateLeaderboardTemplateCommandHandler : IRequestHandler<UpdateLeaderboardTemplateCommand>
{
    private readonly ILeaderboardTemplateRepository _leaderboardTemplateRepository;
    public UpdateLeaderboardTemplateCommandHandler(ILeaderboardTemplateRepository leaderboardTemplateRepository)
    {
        _leaderboardTemplateRepository = leaderboardTemplateRepository;
    }

    public async Task Handle(UpdateLeaderboardTemplateCommand request, CancellationToken cancellationToken)
    {
        var leaderboard = await _leaderboardTemplateRepository.Query().Include(x => x.LeaderboardTemplatePrizes).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (leaderboard == default)
            throw new Exception("Leaderboard not found");

        leaderboard.Update(request.Name, request.Description, request.StartTime, request.EndIn, request.StartIn, request.AnnounceIn);
        foreach (var prize in request.Prizes)
        {
            leaderboard.UpdateLeaderboardPrizes(prize.Id, prize.StartRank, prize.EndRank, prize.PrizeId, prize.Amount);
        }

        _leaderboardTemplateRepository.Update(leaderboard);
        await _leaderboardTemplateRepository.SaveChangesAsync(cancellationToken);
    }
}