using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;

namespace OnAim.Admin.APP.Features.RoleFeatures.Queries.GetAll;

public class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, ApplicationResult>
{
    private readonly IRoleService _roleService;

    public GetAllRolesQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<ApplicationResult> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var result = await _roleService.GetAll(request.Filter);

        return new ApplicationResult { Success = result.Success, Data = result.Data };
    }
}
