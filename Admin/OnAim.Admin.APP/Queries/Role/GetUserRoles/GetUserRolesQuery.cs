using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.Role.GetUserRoles
{
    public record GetUserRolesQuery(int Id) : IRequest<ApplicationResult>;
}
