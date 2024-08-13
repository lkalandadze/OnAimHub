using MediatR;
using OnAim.Admin.APP.Models;

namespace OnAim.Admin.APP.Queries.Role.GetAll
{
    public sealed record GetAllRolesQuery() : IRequest<ApplicationResult>;
}
