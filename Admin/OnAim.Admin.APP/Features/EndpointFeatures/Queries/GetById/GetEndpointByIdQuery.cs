using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.Dtos.Endpoint;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Queries.GetById;

public record GetEndpointByIdQuery(int Id) : IQuery<ApplicationResult<EndpointResponseModel>>;
