using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetCalendar;

public record GetCalendarQuery(DateTimeOffset? StartDate, DateTimeOffset? EndDate) : IQuery<object>;
