using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetUserLogs;

public record GetUserLogsQuery(int Id, AuditLogFilter Filter) : IQuery<ApplicationResult>;

public record AuditLogFilter(
    List<string>? Categories,
    List<string>? Actions,
    DateTimeOffset? DateFrom,
    DateTimeOffset? DateTo,
    int? PageNumber,
    int? PageSize
    );
