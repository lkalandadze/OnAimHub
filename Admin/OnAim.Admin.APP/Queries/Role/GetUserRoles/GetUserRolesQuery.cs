using MediatR;
using OnAim.Admin.APP.Models;

namespace OnAim.Admin.APP.Queries.Role.GetUserRoles
{
    public record GetUserRolesQuery(string Id) : IRequest<ApplicationResult>;
}
