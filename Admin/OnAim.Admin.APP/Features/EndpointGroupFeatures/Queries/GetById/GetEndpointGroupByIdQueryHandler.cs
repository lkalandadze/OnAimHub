using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.AdminServices.EndpointGroup;
using OnAim.Admin.Contracts.Dtos.EndpointGroup;
namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Queries.GetById;

public class GetEndpointGroupByIdQueryHandler : IQueryHandler<GetEndpointGroupByIdQuery, ApplicationResult<EndpointGroupResponseDto>>
{
    private readonly IEndpointGroupService _endpointGroupService;

    public GetEndpointGroupByIdQueryHandler(IEndpointGroupService endpointGroupService)
    {
        _endpointGroupService = endpointGroupService;
    }
    public async Task<ApplicationResult<EndpointGroupResponseDto>> Handle(GetEndpointGroupByIdQuery request, CancellationToken cancellationToken)
    {
       return await _endpointGroupService.GetById(request.Id);
    }
}
