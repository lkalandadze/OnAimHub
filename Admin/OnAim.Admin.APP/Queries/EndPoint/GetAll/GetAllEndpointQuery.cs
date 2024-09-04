using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.EndPoint.GetAll
{
    public sealed record GetAllEndpointQuery(EndpointFilter Filter) : IQuery<ApplicationResult>;
}
