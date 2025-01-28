using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.AuditLog;
using OnAim.Admin.Contracts.Dtos.Base;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetUserLogs;

public record GetUserLogsQuery(int Id, AuditLogFilter Filter) : IQuery<ApplicationResult<PaginatedResult<AuditLogDto>>>;

public class AuditLogFilter : BaseFilter
{
    public List<string>? Categories { get; set; }
    public List<string>? Actions { get; set; }
    public DateTimeOffset? DateFrom { get; set; }
    public DateTimeOffset? DateTo { get; set; }
}

