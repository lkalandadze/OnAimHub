using Leaderboard.Domain.Abstractions.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Commands.Delete;

public class DeleteLeaderboardRecordCommandHandler : IRequestHandler<DeleteLeaderboardRecordCommand>
{
    private readonly ILeaderboardRecordRepository _leaderboardRecordRepository;
    public DeleteLeaderboardRecordCommandHandler(ILeaderboardRecordRepository leaderboardRecordRepository)
    {
        _leaderboardRecordRepository = leaderboardRecordRepository;
    }
    public async Task Handle(DeleteLeaderboardRecordCommand request, CancellationToken cancellationToken)
    {
        var template = await _leaderboardRecordRepository.Query().FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId, cancellationToken);

        if (template == default)
            throw new Exception("Leaderboard template not found with this correlationId");

        _leaderboardRecordRepository.Delete(template);

        await _leaderboardRecordRepository.SaveChangesAsync(cancellationToken);
    }
}
