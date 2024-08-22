using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.EndpointGroup.GetAll
{
    public class GetAllEndpointGroupQueryHandler : IRequestHandler<GetAllEndpointGroupQuery, ApplicationResult>
    {
        private readonly IEndpointGroupRepository _endpointGroupRepository;

        public GetAllEndpointGroupQueryHandler(IEndpointGroupRepository endpointGroupRepository)
        {
            _endpointGroupRepository = endpointGroupRepository;
        }
        public async Task<ApplicationResult> Handle(GetAllEndpointGroupQuery request, CancellationToken cancellationToken)
        {
            var group = await _endpointGroupRepository.GetAllAsync(request.Filter);

            return new ApplicationResult
            {
                Success = true,
                Data = group,
                Errors = null
            };
        }
    }
}
