using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Models.Request.Role;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IRoleRepository _roleRepository;
        private static Dictionary<string, List<string>>? _endpointsByRoles;

        private async Task<Dictionary<string, List<string>>> GetEndpointsByRolesAsync()
        {
            if (_endpointsByRoles == null)
            {
                var filter = new RoleFilter
                {
                    PageNumber = 1,
                    PageSize = 100
                };
                var roles = await _roleRepository.GetAllRoles(filter);

                _endpointsByRoles = roles.Items.ToDictionary(
                    x => x.Name,
                    x => x.EndpointGroupModels.SelectMany(xx => xx.Endpoints?.Select(xxx => xxx.Name!)!).ToList()
                );
            }

            return _endpointsByRoles;
        }

        public PermissionService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<bool> RolesContainPermission(List<string> roles, string permission)
        {
            var endpointsByRoles = await GetEndpointsByRolesAsync();
            return roles.Any(x => endpointsByRoles.ContainsKey(x) && endpointsByRoles[x].Contains(permission));
        }
    }
}