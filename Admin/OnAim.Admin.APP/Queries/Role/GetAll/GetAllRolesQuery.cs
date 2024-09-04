using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Models.Request.Role;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.Role.GetAll
{
    public sealed record GetAllRolesQuery(RoleFilter Filter) : IQuery<ApplicationResult>;
}
