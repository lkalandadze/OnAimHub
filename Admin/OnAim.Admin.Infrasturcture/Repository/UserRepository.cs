using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Exceptions;
using OnAim.Admin.Infrasturcture.Extensions;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Infrasturcture.Models.Request.User;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Models.Response.EndpointGroup;
using OnAim.Admin.Infrasturcture.Models.Response.Role;
using OnAim.Admin.Infrasturcture.Models.Response.User;
using OnAim.Admin.Infrasturcture.Persistance.Data;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Infrasturcture.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _databaseContext;

        public UserRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<User> Create(User user)
        {
            if (user.Id != 0)
            {
                var exist = GetById(user.Id).Result;

                exist.FirstName = user.FirstName;
                exist.LastName = user.LastName;
                exist.Phone = user.Phone;
                exist.Email = user.Email;
                exist.DateUpdated = SystemDate.Now;

                await _databaseContext.SaveChangesAsync();
            }

            var res = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Email,
                Email = user.Email,
                Password = user.Password,
                Salt = user.Salt,
                Phone = user.Phone,
                DateOfBirth = user.DateOfBirth,
                IsActive = true,
                UserId = user.UserId,
                DateCreated = user.DateCreated,
            };
            _databaseContext.Users.Add(res);
            await _databaseContext.SaveChangesAsync();

            var role = await _databaseContext.Roles.Where(x => x.Name == "DefaultRole").FirstOrDefaultAsync();
            await AssignRoleToUserAsync(res.Id, role.Id);

            return res;
        }

        public async Task AssignRoleToUserAsync(int userId, int roleId)
        {
            var userRole = new UserRole { UserId = userId, RoleId = roleId };
            _databaseContext.UserRoles.Add(userRole);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _databaseContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UsersResponseModel> GetById(int id)
        {
            var user = await _databaseContext.Users
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .ThenInclude(x => x.RoleEndpointGroups)
                .ThenInclude(x => x.EndpointGroup)
                .ThenInclude(x => x.EndpointGroupEndpoints)
                .ThenInclude(x => x.Endpoint)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new UserNotFoundException($"User with ID {id} not found.");
            }

            var result = new UsersResponseModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                IsActive = user.IsActive,
                Roles = user.UserRoles.Select(x => new RoleResponseModel
                {
                    Id = x.RoleId,
                    Name = x.Role.Name,
                    Description = x.Role.Description,
                    EndpointGroupModels = x.Role.RoleEndpointGroups.Select(z => new EndpointGroupModel
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
                    }).ToList(),
                }).ToList()
            };

            return result;
        }

        public async Task<List<RoleResponseModel>> GetUserRolesAsync(int userId)
        {
            var result = await _databaseContext.UserRoles
                                          .Include(x => x.Role)
                                          .ThenInclude(x => x.RoleEndpointGroups)
                                          .ThenInclude(x => x.EndpointGroup)
                                          .ThenInclude(x => x.EndpointGroupEndpoints)
                                          .ThenInclude(x => x.Endpoint)
                                          .Where(ur => ur.UserId == userId)
                                          .ToListAsync();

            var roles = result
                .Select(x => x.Role)
                .Distinct()
                .OrderBy(role => role.Id)
                .Select(role => new RoleResponseModel
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    EndpointGroupModels = role.RoleEndpointGroups.Select(z => new EndpointGroupModel
                    {
                        Id = z.EndpointGroupId,
                        Name = z.EndpointGroup.Name,
                        Description = z.EndpointGroup.Description,
                        Endpoints = z.EndpointGroup.EndpointGroupEndpoints.Select(u => new EndpointRequestModel
                        {
                            Id = u.Endpoint.Id,
                            Name = u.Endpoint.Name,
                            Description = u.Endpoint.Description,

                        }).ToList()
                    }).ToList()
                }).ToList();

            return roles;
        }

        public async Task<IEnumerable<string>> GetUserPermissionsAsync(int userId)
        {
            var roles = await _databaseContext.UserRoles
                                      .Where(ur => ur.UserId == userId)
                                      .Select(ur => ur.Role)
                                      .ToListAsync();

            foreach (var role in roles)
            {
                var rolesss = await _databaseContext.Roles
                    .Include(x => x.RoleEndpointGroups)
                    .ThenInclude(x => x.EndpointGroup)
                    .ThenInclude(x => x.EndpointGroupEndpoints)
                    .ThenInclude(x => x.Endpoint)
                    .SingleOrDefaultAsync(x => x.Id == role.Id);

                foreach (var reg in rolesss.RoleEndpointGroups)
                {
                    if (reg.EndpointGroup == null)
                    {
                        Console.WriteLine("Null EndpointGroup detected for Role: " + role.Name);
                        continue;
                    }

                    foreach (var ege in reg.EndpointGroup.EndpointGroupEndpoints)
                    {
                        if (ege.Endpoint == null)
                        {
                            Console.WriteLine("Null Endpoint detected in EndpointGroup: " + reg.EndpointGroup.Name);
                        }
                    }
                }

            }
            var permissions = roles
                    .Where(role => role != null)
                    .SelectMany(role => role.RoleEndpointGroups)
                    .Where(reg => reg.EndpointGroup != null)
                    .SelectMany(reg => reg.EndpointGroup.EndpointGroupEndpoints)
                    .Where(ege => ege.Endpoint != null && ege.Endpoint.IsEnabled)
                    .Select(ege => ege.Endpoint.Path)
                    .Distinct()
                    .ToList();

            return permissions;
        }

        public async Task<PaginatedResult<UsersResponseModel>> GetAllUser(UserFilter userFilter)
        {
            var query = _databaseContext.Users
                          .Include(x => x.UserRoles)
                          .ThenInclude(x => x.Role)
                          .AsQueryable();

            if (!string.IsNullOrEmpty(userFilter.Name))
            {
                query = query.Where(x => x.FirstName.Contains(userFilter.Name));
            }

            if (!string.IsNullOrEmpty(userFilter.Email))
            {
                query = query.Where(x => x.Email.Contains(userFilter.Email));
            }

            if (userFilter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == userFilter.IsActive);
            }

            if (userFilter.RoleIds != null && userFilter.RoleIds.Any())
            {
                query = query.Where(x => x.UserRoles.Any(ur => userFilter.RoleIds.Contains(ur.RoleId)));
            }

            var totalCount = await query.CountAsync();

            var pageNumber = userFilter.PageNumber ?? 1;
            var pageSize = userFilter.PageSize ?? 25;

            var users = await query
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = users
                .Select(user => new UsersResponseModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    IsActive = user.IsActive,
                    Roles = user.UserRoles.Select(x => new RoleResponseModel
                    {
                        Id = x.RoleId,
                        Name = x.Role.Name,
                        Description = x.Role.Description,
                        IsActive = x.Role.IsActive,
                    }).ToList()
                })
                .ToList();

            return new PaginatedResult<UsersResponseModel>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = result
            };
        }

        public async Task<User> UpdateUser(int id, UpdateUserRequest user)
        {
            var existingUser = await _databaseContext.Users.FindAsync(id);

            if (existingUser == null)
            {
                throw new UserNotFoundException("User not found");
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Phone = user.Phone;
            existingUser.DateUpdated = SystemDate.Now;
            //existingUser.UserId = user.UserId;

            var currentRoles = await _databaseContext.UserRoles
                   .Where(ur => ur.UserId == id)
                   .ToListAsync();

            var currentRoleIds = currentRoles.Select(ur => ur.RoleId).ToHashSet();
            var newRoleIds = user.RoleIds?.ToHashSet() ?? new HashSet<int>();

            var rolesToAdd = newRoleIds.Except(currentRoleIds).ToList();
            foreach (var roleId in rolesToAdd)
            {
                var userRole = new UserRole { UserId = id, RoleId = roleId };
                _databaseContext.UserRoles.Add(userRole);
            }

            var rolesToRemove = currentRoleIds.Except(newRoleIds).ToList();
            foreach (var roleId in rolesToRemove)
            {
                var userRole = await _databaseContext.UserRoles
                    .FirstOrDefaultAsync(ur => ur.UserId == id && ur.RoleId == roleId);
                if (userRole != null)
                {
                    _databaseContext.UserRoles.Remove(userRole);
                }
            }

            await _databaseContext.SaveChangesAsync();

            return existingUser;
        }

        public async Task ResetPassword(int id, string password)
        {
            var user = await GetById(id);

            if (user != null && user.IsActive)
            {
                var salt = EncryptPasswordExtension.Salt();

                string hashed = EncryptPasswordExtension.EncryptPassword(password, salt);

                user.Password = hashed;
                user.Salt = salt;

                await _databaseContext.SaveChangesAsync();
            }
        }
    }
}
