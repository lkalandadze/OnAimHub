using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.AdminServices.EndpointGroup;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.EndpointGroup;
using OnAim.Admin.Contracts.Paging;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Queries.GetAll;

public class GetAllEndpointGroupQueryHandler : IQueryHandler<GetAllEndpointGroupQuery, ApplicationResult<PaginatedResult<EndpointGroupModel>>>
{
    private readonly IEndpointGroupService _endpointGroupService;

    public GetAllEndpointGroupQueryHandler(IEndpointGroupService endpointGroupService)
    {
        _endpointGroupService = endpointGroupService;
    }
    public async Task<ApplicationResult<PaginatedResult<EndpointGroupModel>>> Handle(GetAllEndpointGroupQuery request, CancellationToken cancellationToken)
    {
        return await _endpointGroupService.GetAll(request.Filter);
    }
}
