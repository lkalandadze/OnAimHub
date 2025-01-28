using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.AdminServices.User;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.AuditLog;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetUserLogs;

public class GetUserLogsQueryHandler : IQueryHandler<GetUserLogsQuery, ApplicationResult<PaginatedResult<AuditLogDto>>>
{
    private readonly IUserService _userService;

    public GetUserLogsQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ApplicationResult<PaginatedResult<AuditLogDto>>> Handle(GetUserLogsQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetUserLogs(request.Id, request.Filter);
    } 
}
