using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.EndPoint.GetById
{
    public record GetEndpointByIdQuery(int Id) : IQuery<ApplicationResult>;
}
