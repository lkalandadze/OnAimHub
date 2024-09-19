using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Role;

namespace OnAim.Admin.APP.Queries.Role.GetAll
{
    public sealed record GetAllRolesQuery(RoleFilter Filter) : IQuery<ApplicationResult>;
}
