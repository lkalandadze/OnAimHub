using Leaderboard.Domain.Enum;
using MediatR;

namespace Leaderboard.Application.Features.LeaderboardScheduleFeatures.Commands.Update;

public sealed class UpdateLeaderboardScheduleCommand : IRequest
{
    public int Id { get; set; }
    public LeaderboardScheduleStatus Status { get; set; }
}