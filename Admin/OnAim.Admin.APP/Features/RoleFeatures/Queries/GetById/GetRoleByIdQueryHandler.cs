using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.AdminServices.Role;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Role;

namespace OnAim.Admin.APP.Features.RoleFeatures.Queries.GetById;

public class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, ApplicationResult<RoleResponseModel>>
{
    private readonly IRoleService _roleService;

    public GetRoleByIdQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public async Task<ApplicationResult<RoleResponseModel>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
       return await _roleService.GetById(request.Id);  
    }
}
