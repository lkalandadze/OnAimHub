using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Queries.GetById;

public record GetEndpointByIdQuery(int Id) : IQuery<ApplicationResult>;
