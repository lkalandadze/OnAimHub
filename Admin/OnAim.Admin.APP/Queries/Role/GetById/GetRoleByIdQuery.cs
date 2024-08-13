using MediatR;
using OnAim.Admin.APP.Models;

namespace OnAim.Admin.APP.Queries.Role.GetById
{
    public record GetRoleByIdQuery(string Id) : IRequest<ApplicationResult>;
}
