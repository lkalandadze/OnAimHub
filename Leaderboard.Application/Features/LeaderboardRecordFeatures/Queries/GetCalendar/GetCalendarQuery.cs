using MediatR;

namespace Leaderboard.Application.Features.LeaderboardRecordFeatures.Queries.GetCalendar;

public sealed record GetCalendarQuery(DateTimeOffset? StartDate, DateTimeOffset? EndDate) : IRequest<GetCalendarQueryResponse>;