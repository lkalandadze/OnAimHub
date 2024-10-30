using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetUserLogs;

public record GetUserLogsQuery(int Id, AuditLogFilter Filter) : IQuery<ApplicationResult>;

public record AuditLogFilter(
    List<string>? Categories,
    List<string>? Actions,
    DateTimeOffset? DateFrom,
    DateTimeOffset? DateTo
    ) : BaseFilter;
