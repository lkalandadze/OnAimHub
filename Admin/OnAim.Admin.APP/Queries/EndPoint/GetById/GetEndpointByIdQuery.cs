using MediatR;
using OnAim.Admin.APP.Models;

namespace OnAim.Admin.APP.Queries.EndPoint.GetById
{
    public record GetEndpointByIdQuery(string Id) : IRequest<ApplicationResult>;
}
