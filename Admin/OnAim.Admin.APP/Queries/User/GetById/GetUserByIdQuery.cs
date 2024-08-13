using MediatR;
using OnAim.Admin.APP.Models;

namespace OnAim.Admin.APP.Queries.User.GetById
{
    public sealed record GetUserByIdQuery(string Id) : IRequest<ApplicationResult>;
}
