using Leaderboard.Domain.Enum;
using MediatR;

namespace Leaderboard.Application.Features.LeaderboardScheduleFeatures.Commands.Create;

public sealed class CreateLeaderboardScheduleCommand : IRequest
{
    public RepeatType RepeatType { get; set; } // when should job execute 
    public int? RepeatValue { get; set; } // Holds the repeat interval or day information
    public DateOnly? SpecificDate { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public int LeaderboardTemplateId { get; set; }
}
