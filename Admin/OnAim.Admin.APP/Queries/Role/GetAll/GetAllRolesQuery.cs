using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.Role.GetAll
{
    public sealed record GetAllRolesQuery() : IRequest<ApplicationResult>;
}
