using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Request.Role;
using OnAim.Admin.Infrasturcture.Models.Response.EndpointGroup;
using OnAim.Admin.Infrasturcture.Models.Response.Role;
using OnAim.Admin.Infrasturcture.Persistance.Data;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Infrasturcture.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IEndpointGroupRepository _endpointGroupRepository;
        private readonly IEndpointRepository _endpointRepository;

        public RoleRepository(
            DatabaseContext databaseContext,
            IEndpointGroupRepository endpointGroupRepository,
            IEndpointRepository endpointRepository
            )
        {
            _databaseContext = databaseContext;
            _endpointGroupRepository = endpointGroupRepository;
            _endpointRepository = endpointRepository;
        }

        public async Task AssignRoleToUserAsync(string userId, string roleId)
        {
            var userRole = new UserRole { UserId = userId, RoleId = roleId };
            _databaseContext.UserRoles.Add(userRole);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task DeactivateRoleForUserAsync(string userId, string roleId)
        {
            var userRole = await _databaseContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
            if (userRole != null)
            {
                await _databaseContext.SaveChangesAsync();
            }
        }

        public async Task RemoveRoleFromUserAsync(string userId, string roleId)
        {
            var userRole = await _databaseContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
            if (userRole != null)
            {
                _databaseContext.UserRoles.Remove(userRole);
                await _databaseContext.SaveChangesAsync();
            }
        }

        public async Task<Role> CreateRoleAsync(CreateRoleRequest request)
        {
            var existsName = _databaseContext.Roles.Where(x => x.Name == request.Name).Any();
            if (existsName)
            {
                throw new Exception("Role With That Name ALready Exists");
            }
            var role = new Role
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                DateCreated = SystemDate.Now,
                IsActive = true,
                RoleEndpointGroups = new List<RoleEndpointGroup>(),
                UserId = request.ParentUserId,
            };
            _databaseContext.Roles.Add(role);

            foreach (var item in request.EndpointGroups)
            {
                if (item.Id != null)
                {
                    var ep = await _endpointGroupRepository.GetByIdAsync(item.Id);
                    var res = new RoleEndpointGroup
                    {
                        RoleId = role.Id,
                        EndpointGroupId = ep.Id
                    };
                    _databaseContext.RoleEndpointGroups.Add(res);
                    await _databaseContext.SaveChangesAsync();
                }
                var endpointGroup = new EndpointGroup
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = item.Name,
                    Description = item.Description,
                    IsActive = true,
                    IsEnabled = true,
                    UserId = request.ParentUserId,
                    DateCreated = DateTime.UtcNow,
                    EndpointGroupEndpoints = new List<EndpointGroupEndpoint>()
                };

                _databaseContext.EndpointGroups.Add(endpointGroup);

                var endpoints = await _endpointRepository.GetEndpointsByIdsAsync(item.EndpointIds);

                foreach (var ep in endpoints)
                {
                    var endpointGroupEndpoint = new EndpointGroupEndpoint
                    {
                        EndpointGroupId = endpointGroup.Id,
                        EndpointId = ep.Id
                    };
                    endpointGroup.EndpointGroupEndpoints.Add(endpointGroupEndpoint);
                }

                var roleEndpointGroup = new RoleEndpointGroup
                {
                    RoleId = role.Id,
                    EndpointGroupId = endpointGroup.Id
                };
                role.RoleEndpointGroups.Add(roleEndpointGroup);
            }

            await _databaseContext.SaveChangesAsync();

            return role;
        }

        public async Task<Role> UpdateRoleAsync(string roleId, UpdateRoleRequest request)
        {
            var role = await _databaseContext.Roles
                               .Include(r => r.RoleEndpointGroups)
                               .ThenInclude(reg => reg.EndpointGroup)
                               .ThenInclude(eg => eg.EndpointGroupEndpoints)
                               .ThenInclude(ege => ege.Endpoint)
                               .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
            {
                throw new Exception("Role not found");
            }

            if (role.IsActive == false)
            {
                role.DateUpdated = SystemDate.Now;
                role.IsActive = false;
            }

            role.Name = request.Name;
            role.Description = request.Description;
            role.IsActive = request.IsActive;

            var existingEndpointGroups = role.RoleEndpointGroups.ToList();
            var endpointGroupsToRemove = existingEndpointGroups
                .Where(eg => !request.EndpointGroups.Any(egReq => egReq.Id == eg.EndpointGroupId))
                .ToList();

            foreach (var egToRemove in endpointGroupsToRemove)
            {
                _databaseContext.RoleEndpointGroups.Remove(egToRemove);
                var endpointGroup = await _databaseContext.EndpointGroups
                    .Include(eg => eg.EndpointGroupEndpoints)
                    .FirstOrDefaultAsync(eg => eg.Id == egToRemove.EndpointGroupId);

                if (endpointGroup != null)
                {
                    _databaseContext.EndpointGroups.Remove(endpointGroup);
                }
            }

            foreach (var reqGroup in request.EndpointGroups)
            {
                var existingGroup = role.RoleEndpointGroups.FirstOrDefault(eg => eg.EndpointGroupId == reqGroup.Id);

                if (existingGroup == null)
                {
                    var endpointGroup = await _endpointGroupRepository.GetByIdAsync(reqGroup.Id);
                    var roleEndpointGroup = new RoleEndpointGroup
                    {
                        RoleId = role.Id,
                        EndpointGroupId = endpointGroup.Id
                    };
                    role.RoleEndpointGroups.Add(roleEndpointGroup);

                    _databaseContext.RoleEndpointGroups.Add(roleEndpointGroup);

                    foreach (var endpointId in reqGroup.EndpointIds)
                    {
                        var endpoint = await _endpointRepository.GetEndpointById(endpointId);

                        var endpointGroupEndpoint = new EndpointGroupEndpoint
                        {
                            EndpointGroupId = endpointGroup.Id,
                            EndpointId = endpoint.Id
                        };

                        endpointGroup.EndpointGroupEndpoints.Add(endpointGroupEndpoint);
                        _databaseContext.EndpointGroupEndpoints.Add(endpointGroupEndpoint);
                    }

                    _databaseContext.EndpointGroups.Add(endpointGroup);
                }
                else
                {
                    var endpointGroup = await _databaseContext.EndpointGroups
                        .Include(eg => eg.EndpointGroupEndpoints)
                        .FirstOrDefaultAsync(eg => eg.Id == reqGroup.Id);

                    var existingEndpoints = endpointGroup.EndpointGroupEndpoints.ToList();
                    var endpointsToRemove = existingEndpoints
                        .Where(ege => !reqGroup.EndpointIds.Contains(ege.EndpointId))
                        .ToList();

                    foreach (var egeToRemove in endpointsToRemove)
                    {
                        _databaseContext.EndpointGroupEndpoints.Remove(egeToRemove);
                        var endpoint = await _databaseContext.Endpoints
                            .FirstOrDefaultAsync(e => e.Id == egeToRemove.EndpointId);
                        if (endpoint != null)
                        {
                            _databaseContext.Endpoints.Remove(endpoint);
                        }
                    }

                    foreach (var endpointId in reqGroup.EndpointIds)
                    {
                        if (!existingEndpoints.Any(ege => ege.EndpointId == endpointId))
                        {
                            var endpoint = await _endpointRepository.GetEndpointById(endpointId);
                            var endpointGroupEndpoint = new EndpointGroupEndpoint
                            {
                                EndpointGroupId = endpointGroup.Id,
                                EndpointId = endpoint.Id
                            };
                            endpointGroup.EndpointGroupEndpoints.Add(endpointGroupEndpoint);
                            _databaseContext.EndpointGroupEndpoints.Add(endpointGroupEndpoint);
                        }
                    }
                }
            }

            await _databaseContext.SaveChangesAsync();

            return role;
        }

        public async Task<RoleResponseModel> GetRoleById(string roleId)
        {
            var role = await _databaseContext.Roles
                .Include(x => x.RoleEndpointGroups)
                .ThenInclude(x => x.EndpointGroup)
                .ThenInclude(x => x.EndpointGroupEndpoints)
                .ThenInclude(x => x.Endpoint)
                .FirstOrDefaultAsync(x => x.Id == roleId);

            var result = new RoleResponseModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                DateCreated = role.DateCreated,
                EndpointGroupModels = role.RoleEndpointGroups.Select(x => new EndpointGroupModel
                {
                    Id = x.EndpointGroupId,
                    Name = x.EndpointGroup.Name,
                    Description = x.EndpointGroup.Description,
                    DateCreated = x.EndpointGroup.DateCreated,
                    Endpoints = x.EndpointGroup.EndpointGroupEndpoints.Select(x => new Endpoint
                    {
                        Id = x.EndpointId,
                        Name = x.Endpoint.Name,
                        Description = x.Endpoint.Description,
                        Path = x.Endpoint.Path,
                        DateCreated = x.Endpoint.DateCreated,
                    }).ToList(),
                }).ToList(),
            };

            return result;
        }

        public async Task<List<RoleResponseModel>> GetAllRolesAsync()
        {
            var result = await _databaseContext.Roles.Include(r => r.RoleEndpointGroups)
                                  .ThenInclude(reg => reg.EndpointGroup)
                                  .ThenInclude(x => x.EndpointGroupEndpoints)
                                  .ThenInclude(x => x.Endpoint)
                                  .ToListAsync();

            var roles = result
                        .OrderBy(x => x.Id)
                        .Select(x => new RoleResponseModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Description = x.Description,
                            IsActive = x.IsActive,
                            EndpointGroupModels = x.RoleEndpointGroups.Select(z => new EndpointGroupModel
                            {
                                Id = z.EndpointGroupId,
                                Name = z.EndpointGroup.Name,
                                Description = z.EndpointGroup.Description,
                                Endpoints = z.EndpointGroup.EndpointGroupEndpoints.Select(u => new Endpoint
                                {
                                    Id = u.EndpointId,
                                    Name = u.Endpoint.Name,
                                    Path = u.Endpoint.Path,
                                    Description = u.Endpoint.Description,
                                }).ToList()
                            }).ToList()
                        }).ToList();

            return roles;
        }

        public async Task<IEnumerable<Role>> GetRolesByIdsAsync(IEnumerable<string> roleIds)
        {
            return await _databaseContext.Roles.Where(r => roleIds.Contains(r.Id)).ToListAsync();
        }
    }
}
