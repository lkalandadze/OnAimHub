using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Queries.EndpointGroup.GetAll
{
    public class GetAllEndpointGroupQueryHandler : Handler, IRequestHandler<GetAllEndpointGroupQuery, ApplicationResult>
    {
        private readonly IEndpointGroupRepository _endpointGroupRepository;

        public GetAllEndpointGroupQueryHandler(IEndpointGroupRepository endpointGroupRepository)
        {
            _endpointGroupRepository = endpointGroupRepository;
        }
        public async Task<ApplicationResult> Handle(GetAllEndpointGroupQuery request, CancellationToken cancellationToken)
        {
            var group = await _endpointGroupRepository.GetAllAsync();

            return Ok(group);
        }
    }

    public class Handler
    {
        public ApplicationResult Ok(dynamic data)
        {
            return ApplicationResult.DefaultWithSuccess(data);
        }

        public ApplicationResult Error(string error)
        {
            return ApplicationResult.DefaultWithError(error);
        }
    }
}
