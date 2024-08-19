using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndPoint.Update
{
    public sealed class UpdateEndpointCommandHandler : IRequestHandler<UpdateEndpointCommand, ApplicationResult>
    {
        private readonly IEndpointRepository _endpointRepository;

        public UpdateEndpointCommandHandler(IEndpointRepository endpointRepository)
        {
            _endpointRepository = endpointRepository;
        }
        public async Task<ApplicationResult> Handle(UpdateEndpointCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
