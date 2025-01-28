using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.AdminServices.User;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.User;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetById;

public sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, ApplicationResult<GetUserModel>>
{
    private readonly IUserService _userService;

    public GetUserByIdQueryHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<ApplicationResult<GetUserModel>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
       return await _userService.GetById(request.Id);
    }
}
