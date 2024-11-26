using Leaderboard.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Application.Features.LeaderboardTemplateFeatures.Commands.Delete;

public class DeleteLeaderboardTemplateCommandHandler : IRequestHandler<DeleteLeaderboardTemplateCommand>
{
    private readonly ILeaderboardTemplateRepository _leaderboardTemplateRepository;
    public DeleteLeaderboardTemplateCommandHandler(ILeaderboardTemplateRepository leaderboardTemplateRepository)
    {
        _leaderboardTemplateRepository = leaderboardTemplateRepository;
    }
    public async Task Handle(DeleteLeaderboardTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _leaderboardTemplateRepository.Query().FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId, cancellationToken);

        if (template == default)
            throw new Exception("Leaderboard template not found with this correlationId");

        _leaderboardTemplateRepository.Delete(template);

        await _leaderboardTemplateRepository.SaveChangesAsync(cancellationToken);
    }
}
