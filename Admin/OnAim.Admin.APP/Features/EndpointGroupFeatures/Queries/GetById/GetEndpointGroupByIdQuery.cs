using MediatR;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.EndpointGroup;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Queries.GetById;

public record GetEndpointGroupByIdQuery(int Id) : IQuery<ApplicationResult<EndpointGroupResponseDto>>;
