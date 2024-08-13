using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Commands.EndPoint.Disable
{
    public class DisableEndpointCommandHandler : IRequestHandler<DisableEndpointCommand, ApplicationResult>
    {
        private readonly IEndpointRepository _endpointRepository;

        public DisableEndpointCommandHandler(IEndpointRepository endpointRepository)
        {
            _endpointRepository = endpointRepository;
        }
        public async Task<ApplicationResult> Handle(DisableEndpointCommand request, CancellationToken cancellationToken)
        {
            var result = await _endpointRepository.DisableEndpointAsync(request.EndpointId);

            return new ApplicationResult
            {
                Success = true,
                Data = result,
                Errors = null
            };
        }
    }
}
