using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IConfigurationRepository<Infrasturcture.Entities.UserRole> _userRoleRepository;
        private readonly IRepository<Role> _repository;
        private static Dictionary<string, List<string>>? _endpointsByRoles;

        private async Task<Dictionary<string, List<string>>> GetEndpointsByRolesAsync()
        {
            if (_endpointsByRoles == null)
            {
                var roles = await _roleRepository.GetAllRoles();

                _endpointsByRoles = roles.Items
                    .Where(x => x.IsActive == true)
                    .ToDictionary(
                    x => x.Name,
                    x => x.EndpointGroupModels.SelectMany(xx => xx.Endpoints?.Select(xxx => xxx.Name!)!).ToList()
                );
            }

            return _endpointsByRoles;
        }

        public PermissionService(
            IRoleRepository roleRepository,
            IConfigurationRepository<Infrasturcture.Entities.UserRole> userRoleRepository,
            IRepository<Infrasturcture.Entities.Role> repository)
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _repository = repository;
        }

        public async Task<bool> RolesContainPermission(List<string> roles, string permission)
        {
            //var userId = HttpContextAccessorProvider.HttpContextAccessor.GetUserId();

            //var activeRoleNames = await _userRoleRepository
            //               .Query(ur => ur.UserId == userId && ur.IsActive)
            //               .Join(_repository.Query(), ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
            //               .ToListAsync();

            //var endpointsByRoles = await GetEndpointsByRolesAsync();

            //return roles
            //    .Where(role => activeRoleNames.Contains(role))
            //    .Any(role => endpointsByRoles.ContainsKey(role) && endpointsByRoles[role].Contains(permission));


            var endpointsByRoles = await GetEndpointsByRolesAsync();
            return roles.Any(x => endpointsByRoles.ContainsKey(x) && endpointsByRoles[x].Contains(permission));
        }
    }
}