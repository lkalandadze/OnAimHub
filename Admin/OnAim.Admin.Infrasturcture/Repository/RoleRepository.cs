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

        public RoleRepository(
            DatabaseContext databaseContext
            )
        {
            _databaseContext = databaseContext;
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
                //UserId = request.ParentUserId,
            };
            _databaseContext.Roles.Add(role);
            await _databaseContext.SaveChangesAsync();

            foreach (var group in request.EndpointGroupIds)
            {
                var epgroup = await _databaseContext.EndpointGroups.FindAsync(group);

                if (!epgroup.IsEnabled)
                {
                    throw new Exception("EndpointGroup Is Disabled!");
                }

                var roleEndpointGroup = new RoleEndpointGroup
                {
                    EndpointGroupId = epgroup.Id,
                    RoleId = role.Id
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

            if (request.EndpointGroupIds != null)
            {
                foreach (var item in request.EndpointGroupIds)
                {
                    var group = await _databaseContext.EndpointGroups.FindAsync(item);

                    if (group == null)
                    {
                        throw new Exception("Permission Group Not Found");
                    }

                    var roleEndpointGroup = new RoleEndpointGroup
                    {
                        EndpointGroupId = group.Id,
                        RoleId = role.Id
                    };

                    role.RoleEndpointGroups.Add(roleEndpointGroup);
                    await _databaseContext.SaveChangesAsync();
                }
            }

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

        //For permission check
        public async Task<PaginatedResult<RoleResponseModel>> GetAllRoles(RoleFilter filter)
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

            var pageNumber = filter.PageNumber ?? 1;
            var pageSize = filter.PageSize ?? 25;

            var roles = await query
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = roles
            };
        }

        public async Task<PaginatedResult<RoleResponseModel>> GetAllRolesAsync(RoleFilter filter)
        {
            var query = _databaseContext.Roles
                .Include(r => r.RoleEndpointGroups)
                    .ThenInclude(reg => reg.EndpointGroup)
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

            var pageNumber = filter.PageNumber ?? 1;
            var pageSize = filter.PageSize ?? 25;

            var roles = await query
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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
                        IsActive = z.EndpointGroup.IsActive,
                        DateCreated = z.EndpointGroup.DateCreated,
                        DateDeleted = z.EndpointGroup.DateDeleted,
                        DateUpdated = z.EndpointGroup.DateUpdated,
                    }).ToList()
                }).ToListAsync();

            return new PaginatedResult<RoleResponseModel>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
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
                role.IsActive = false;

                await _databaseContext.SaveChangesAsync();
            }
        }
    }
}
