using MediatR;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.EndpointGroup.GetAll
{
    public record GetAllEndpointGroupQuery(EndpointFilter Filter) : IRequest<ApplicationResult>;
}
