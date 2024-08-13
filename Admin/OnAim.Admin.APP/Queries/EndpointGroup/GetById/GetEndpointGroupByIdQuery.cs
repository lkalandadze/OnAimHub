using MediatR;
using OnAim.Admin.APP.Models;

namespace OnAim.Admin.APP.Queries.EndpointGroup.GetById
{
    public record GetEndpointGroupByIdQuery(string Id) : IRequest<ApplicationResult>;
}
