using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Base;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetUserLogs;

public record GetUserLogsQuery(int Id, AuditLogFilter Filter) : IQuery<ApplicationResult>;

public class AuditLogFilter : BaseFilter
{
    public List<string>? Categories { get; set; }
    public List<string>? Actions { get; set; }
    public DateTimeOffset? DateFrom { get; set; }
    public DateTimeOffset? DateTo { get; set; }
}

