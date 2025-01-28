using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.AdminServices.Role;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.Contracts.Dtos.Role;

namespace OnAim.Admin.APP.Features.RoleFeatures.Queries.GetAll;

public class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, ApplicationResult<PaginatedResult<RoleShortResponseModel>>>
{
    private readonly IRoleService _roleService;

    public GetAllRolesQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<ApplicationResult<PaginatedResult<RoleShortResponseModel>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        return await _roleService.GetAll(request.Filter);
    }
}
