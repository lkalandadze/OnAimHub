using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Services.Admin.AuthServices;

public class PermissionService : IPermissionService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IConfigurationRepository<UserRole> _userRoleRepository;
    private readonly IRepository<OnAim.Admin.Domain.Entities.Role> _repository;
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
        IConfigurationRepository<UserRole> userRoleRepository,
        IRepository<OnAim.Admin.Domain.Entities.Role> repository)
    {
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _repository = repository;
    }

    public async Task<bool> RolesContainPermission(List<string> roles, string permission)
    {
        var endpointsByRoles = await GetEndpointsByRolesAsync();
        return roles.Any(x => endpointsByRoles.ContainsKey(x) && endpointsByRoles[x].Contains(permission));
    }
}