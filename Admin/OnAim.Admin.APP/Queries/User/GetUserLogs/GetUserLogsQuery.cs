using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.User.GetUserLogs
{
    public record GetUserLogsQuery(int Id, AuditLogFilter Filter) : IQuery<ApplicationResult>;

    public record AuditLogFilter(
        List<string>? Types, 
        List<string>? Actions,
        DateTimeOffset? DateFrom,
        DateTimeOffset? DateTo, 
        int? PageNumber, 
        int? PageSize
        );
}
