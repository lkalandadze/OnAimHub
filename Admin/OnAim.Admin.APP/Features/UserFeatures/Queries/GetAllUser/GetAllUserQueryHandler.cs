using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetAllUser;

public sealed class GetAllUserQueryHandler : IQueryHandler<GetAllUserQuery, ApplicationResult>
{
    private readonly IUserService _userService;

    public GetAllUserQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ApplicationResult> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var result = await _userService.GetAll(request.UserFilter);

        return new ApplicationResult
        {
            Success = result.Success,
            Data = result.Data
        };
    }
}
