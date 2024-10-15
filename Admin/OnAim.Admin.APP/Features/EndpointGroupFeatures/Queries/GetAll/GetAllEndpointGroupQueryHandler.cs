using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Queries.GetAll;

public class GetAllEndpointGroupQueryHandler : IQueryHandler<GetAllEndpointGroupQuery, ApplicationResult>
{
    private readonly IEndpointGroupService _endpointGroupService;

    public GetAllEndpointGroupQueryHandler(IEndpointGroupService endpointGroupService)
    {
        _endpointGroupService = endpointGroupService;
    }
    public async Task<ApplicationResult> Handle(GetAllEndpointGroupQuery request, CancellationToken cancellationToken)
    {
        var result = await _endpointGroupService.GetAll(request.Filter);

        return new ApplicationResult
        {
            Success = true,
            Data = result
        };
    }
}
