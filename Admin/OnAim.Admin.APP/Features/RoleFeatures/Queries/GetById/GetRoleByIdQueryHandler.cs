using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.RoleFeatures.Queries.GetById;

public class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, ApplicationResult>
{
    private readonly IRoleService _roleService;

    public GetRoleByIdQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }
    public async Task<ApplicationResult> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
       var result = await _roleService.GetById(request.Id);  

        return new ApplicationResult
        {
            Success = result.Success,
            Data = result.Data,
        };
    }
}
