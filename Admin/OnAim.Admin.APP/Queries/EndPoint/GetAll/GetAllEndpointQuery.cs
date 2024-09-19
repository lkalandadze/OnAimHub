using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Endpoint;

namespace OnAim.Admin.APP.Queries.EndPoint.GetAll
{
    public sealed record GetAllEndpointQuery(EndpointFilter Filter) : IQuery<ApplicationResult>;
}
