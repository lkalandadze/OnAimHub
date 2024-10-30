using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetUserLogs;

public class GetUserLogsQueryHandler : IQueryHandler<GetUserLogsQuery, ApplicationResult>
{
    private readonly IUserService _userService;

    public GetUserLogsQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ApplicationResult> Handle(GetUserLogsQuery request, CancellationToken cancellationToken)
    {
        var result = await _userService.GetUserLogs(request.Id, request.Filter);

        return new ApplicationResult
        {
            Success = result.Success,
            Data = result.Data
        };
    } 
}
