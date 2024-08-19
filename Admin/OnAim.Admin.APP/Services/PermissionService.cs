using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IEndpointGroupRepository _endpointGroupRepository;
        private readonly IEndpointRepository _endpointRepository;

        public PermissionService(IRoleRepository roleRepository, IEndpointGroupRepository endpointGroupRepository, IEndpointRepository endpointRepository)
        {
            _roleRepository = roleRepository;
            _endpointGroupRepository = endpointGroupRepository;
            _endpointRepository = endpointRepository;
        }

        public async Task<bool> HasPermissionForRoleAsync(string role, string permission)
        {
            var roleEntity = await _roleRepository.GetRoleByName(role);

            if (roleEntity == null)
            {
                return false;
            }

            foreach (var endpointGroup in roleEntity.EndpointGroupModels)
            {
                var endpointGroups = await _endpointGroupRepository.GetByIdAsync(endpointGroup.Id);

                if (endpointGroups == null)
                {
                    continue;
                }

                foreach (var ep in endpointGroups.EndpointGroupEndpoints)
                {
                    var endpoint = await _endpointRepository.GetEndpointById(ep.EndpointId);

                    if (endpoint != null && endpoint.Name == permission)
                    {
                        return true;
                    }
                }

            }

            return false;
        }
    }
}
