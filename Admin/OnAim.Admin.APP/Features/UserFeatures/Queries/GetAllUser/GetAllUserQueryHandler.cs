using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.AdminServices.User;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.User;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetAllUser;

public sealed class GetAllUserQueryHandler : IQueryHandler<GetAllUserQuery, ApplicationResult<PaginatedResult<UsersModel>>>
{
    private readonly IUserService _userService;

    public GetAllUserQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ApplicationResult<PaginatedResult<UsersModel>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetAll(request.UserFilter);
    }
}
