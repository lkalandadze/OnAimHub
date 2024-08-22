using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Infrasturcture.Models.Request.Role;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Models.Response.EndpointGroup;
using OnAim.Admin.Infrasturcture.Models.Response.Role;
using OnAim.Admin.Infrasturcture.Persistance.Data;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.Models;
using static OnAim.Admin.Infrasturcture.Exceptions.Exceptions;

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

        public async Task AssignRoleToUserAsync(int userId, int roleId)
        {
            var userRole = new UserRole { UserId = userId, RoleId = roleId };
            _databaseContext.UserRoles.Add(userRole);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task RemoveRoleFromUserAsync(int userId, int roleId)
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
                Name = request.Name,
                Description = request.Description,
                DateCreated = SystemDate.Now,
                IsActive = true,
                RoleEndpointGroups = new List<RoleEndpointGroup>(),
                UserId = request.ParentUserId,
            };
            _databaseContext.Roles.Add(role);
            await _databaseContext.SaveChangesAsync();

            foreach (var item in request.EndpointGroups)
            {
                if (item.Id != 0)
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
                    Name = item.Name,
                    Description = item.Description,
                    IsActive = true,
                    IsEnabled = true,
                    UserId = request.ParentUserId,
                    DateCreated = DateTime.UtcNow,
                    EndpointGroupEndpoints = new List<EndpointGroupEndpoint>()
                };

                foreach (var endpointId in item.EndpointIds)
                {
                    var endpoint = _endpointRepository.GetEndpointById(endpointId).Result;

                    if (!endpoint.IsEnabled)
                    {
                        throw new Exception("Endpoint Is Disabled!");
                    }

                    var endpointGroupEndpoint = new EndpointGroupEndpoint
                    {
                        Endpoint = endpoint,
                        EndpointGroup = endpointGroup
                    };

                    endpointGroup.EndpointGroupEndpoints.Add(endpointGroupEndpoint);
                }

                await _endpointGroupRepository.AddAsync(endpointGroup);
                await _endpointGroupRepository.SaveChangesAsync();

                var roleEndpointGroup = new RoleEndpointGroup
                {
                    RoleId = role.Id,
                    EndpointGroupId = endpointGroup.Id
                };
                role.RoleEndpointGroups.Add(roleEndpointGroup);
                await _databaseContext.SaveChangesAsync();
            }

            await _databaseContext.SaveChangesAsync();

            return role;
        }

        public async Task<Role> UpdateRoleAsync(int roleId, UpdateRoleRequest request)
        {
            var role = await _databaseContext.Roles
                               .Include(r => r.RoleEndpointGroups)
                               .ThenInclude(reg => reg.EndpointGroup)
                               .ThenInclude(eg => eg.EndpointGroupEndpoints)
                               .ThenInclude(ege => ege.Endpoint)
                               .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
            {
                throw new RoleNotFoundException("Role not found");
            }

            if (!role.IsActive)
            {
                role.DateUpdated = SystemDate.Now;
                role.IsActive = false;
            }

            role.Name = request.Name;
            role.Description = request.Description;

            //if (request.EndpointGroups != null)
            //{
            //    foreach (var item in request.EndpointGroups)
            //    {
            //        var group = await _endpointGroupRepository.GetByIdAsync(item.Id);

            //        if (group == null)
            //        {
            //            throw new Exception("Permission Group Not Found");
            //        }

            //        if (!string.IsNullOrEmpty(item.Name) || !string.IsNullOrEmpty(item.Description))
            //        {
            //            group.Name = item.Name;
            //            group.Description = item.Description;

            //            await _endpointGroupRepository.UpdateAsync(group);
            //        }

            //        var currentEndpointIds = group.EndpointGroupEndpoints.Select(ep => ep.EndpointId).ToList();
            //        var newEndpointIds = item.EndpointIds ?? new List<int>();

            //        var endpointsToAdd = newEndpointIds.Except(currentEndpointIds).ToList();
            //        var endpointsToRemove = currentEndpointIds.Except(newEndpointIds).ToList();

            //        foreach (var endpointId in endpointsToAdd)
            //        {
            //            var endpoint = await _endpointRepository.GetEndpointById(endpointId);

            //            if (endpoint != null)
            //            {
            //                await _endpointGroupRepository.AddEndpoint(group, endpoint);
            //            }
            //        }

            //        foreach (var endpointId in endpointsToRemove)
            //        {
            //            var endpoint = await _endpointRepository.GetEndpointById(endpointId);

            //            if (endpoint != null)
            //            {
            //                await _endpointGroupRepository.RemoveEndpoint(group, endpoint);
            //            }
            //        }

            //        await _endpointGroupRepository.SaveChangesAsync();
            //    }
            //}

            await _databaseContext.SaveChangesAsync();

            return role;
        }

        public async Task<RoleResponseModel> GetRoleById(int roleId)
        {
            var role = await _databaseContext.Roles
                .Include(x => x.RoleEndpointGroups)
                .ThenInclude(x => x.EndpointGroup)
                .ThenInclude(x => x.EndpointGroupEndpoints)
                .ThenInclude(x => x.Endpoint)
                .FirstOrDefaultAsync(x => x.Id == roleId);

            if (role == null)
            {
                throw new RoleNotFoundException("Role Not Found.");
            }

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
                    Endpoints = x.EndpointGroup.EndpointGroupEndpoints.Select(x => new EndpointRequestModel
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

        public async Task<RoleResponseModel> GetRoleByName(string roleName)
        {
            var role = await _databaseContext.Roles
                .Include(x => x.RoleEndpointGroups)
                .ThenInclude(x => x.EndpointGroup)
                .ThenInclude(x => x.EndpointGroupEndpoints)
                .ThenInclude(x => x.Endpoint)
                .FirstOrDefaultAsync(x => x.Name == roleName);

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
                    Endpoints = x.EndpointGroup.EndpointGroupEndpoints.Select(x => new EndpointRequestModel
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

        public async Task<PaginatedResult<RoleResponseModel>> GetAllRolesAsync(RoleFilter filter)
        {
            var query = _databaseContext.Roles
                .Include(r => r.RoleEndpointGroups)
                    .ThenInclude(reg => reg.EndpointGroup)
                        .ThenInclude(eg => eg.EndpointGroupEndpoints)
                            .ThenInclude(ege => ege.Endpoint)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(x => x.Name.Contains(filter.Name));
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive);
            }

            var totalCount = await query.CountAsync();

            var roles = await query
                .OrderBy(x => x.Id)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
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
                        Endpoints = z.EndpointGroup.EndpointGroupEndpoints.Select(u => new EndpointRequestModel
                        {
                            Id = u.EndpointId,
                            Name = u.Endpoint.Name,
                            Path = u.Endpoint.Path,
                            Description = u.Endpoint.Description,
                        }).ToList()
                    }).ToList()
                }).ToListAsync();

            return new PaginatedResult<RoleResponseModel>
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount,
                Items = roles
            };
        }

        public async Task<IEnumerable<Role>> GetRolesByIdsAsync(IEnumerable<int> roleIds)
        {
            return await _databaseContext.Roles.Where(r => roleIds.Contains(r.Id)).ToListAsync();
        }

        public async Task DeleteRole(int roleId)
        {
            var role = await _databaseContext.Roles
                .Include(r => r.UserRoles)
                .Include(r => r.RoleEndpointGroups)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role != null)
            {
                _databaseContext.UserRoles.RemoveRange(role.UserRoles);

                _databaseContext.RoleEndpointGroups.RemoveRange(role.RoleEndpointGroups);

                _databaseContext.Roles.Remove(role);

                await _databaseContext.SaveChangesAsync();
            }
        }
    }
}
