using MediatR;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Commands.EndpointGroup.Update
{
    public class UpdateEndpointGroupCommandHandler : IRequestHandler<UpdateEndpointGroupCommand, ApplicationResult>
    {
        private readonly IEndpointGroupRepository _endpointGroupRepository;
        private readonly IEndpointRepository _endpointRepository;

        public UpdateEndpointGroupCommandHandler(
            IEndpointGroupRepository endpointGroupRepository,
            IEndpointRepository endpointRepository
            )
        {
            _endpointGroupRepository = endpointGroupRepository;
            _endpointRepository = endpointRepository;
        }
        public async Task<ApplicationResult> Handle(UpdateEndpointGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _endpointGroupRepository.GetByIdAsync(request.Id);

            if (group == null)
            {
                throw new Exception("Permission Group Not Found");
            }

            var currentEndpointIds = group.EndpointGroupEndpoints.Select(ep => ep.EndpointId).ToList();
            var newEndpointIds = request.model.EndpointIds ?? new List<int>();

            var endpointsToAdd = newEndpointIds.Except(currentEndpointIds).ToList();
            var endpointsToRemove = currentEndpointIds.Except(newEndpointIds).ToList();

            foreach (var endpointId in endpointsToAdd)
            {
                var endpoint = await _endpointRepository.GetEndpointById(endpointId);

                if (endpoint != null)
                {
                    await _endpointGroupRepository.AddEndpoint(group, endpoint);
                }
            }

            foreach (var endpointId in endpointsToRemove)
            {
                var endpoint = await _endpointRepository.GetEndpointById(endpointId);

                if (endpoint != null)
                {
                    await _endpointGroupRepository.RemoveEndpoint(group, endpoint);
                }
            }

            await _endpointGroupRepository.SaveChangesAsync();

            return new ApplicationResult
            {
                Success = true,
                Data = group,
                Errors = null
            };
        }
    }
}
