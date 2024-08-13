using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Queries.EndPoint.GetById
{
    public class GetEndpointByIdQueryHandler : IRequestHandler<GetEndpointByIdQuery, ApplicationResult>
    {
        private readonly IEndpointRepository _endpointRepository;

        public GetEndpointByIdQueryHandler(IEndpointRepository endpointRepository)
        {
            _endpointRepository = endpointRepository;
        }
        public async Task<ApplicationResult> Handle(GetEndpointByIdQuery request, CancellationToken cancellationToken)
        {
            var endpoint = await _endpointRepository.GetEndpointById(request.Id);

            return new ApplicationResult
            {
                Success = true,
                Data = endpoint,
                Errors = null
            };
        }
    }
}
