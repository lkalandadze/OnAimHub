using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.AdminServices.User;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.GetById;

public sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, ApplicationResult>
{
    private readonly IUserService _userService;

    public GetUserByIdQueryHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<ApplicationResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
       var result = await _userService.GetById(request.Id);

        return new ApplicationResult
        {
            Success = result.Success,
            Data = result.Data,
        };
    }
}
