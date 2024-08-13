using MediatR;
using OnAim.Admin.APP.Models;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.APP.Commands.Role.Update
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ApplicationResult>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IEndpointGroupRepository _endpointGroupRepository;
        private readonly IEndpointRepository _endpointRepository;

        public UpdateRoleCommandHandler(
            IRoleRepository roleRepository,
            IEndpointGroupRepository endpointGroupRepository,
            IEndpointRepository endpointRepository
            )
        {
            _roleRepository = roleRepository;
            _endpointGroupRepository = endpointGroupRepository;
            _endpointRepository = endpointRepository;
        }
        public async Task<ApplicationResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.UpdateRoleAsync(request.Id, request.Model);

            return new ApplicationResult
            {
                Success = true,
                Data = role,
                Errors = null
            };
        }
    }
}
