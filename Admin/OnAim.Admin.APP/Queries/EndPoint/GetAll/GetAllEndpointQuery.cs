using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Models.Request.Role;

namespace OnAim.Admin.APP.Queries.EndPoint.GetAll
{
    public sealed record GetAllEndpointQuery(RoleFilter RoleFilter) : IRequest<ApplicationResult>;
}
