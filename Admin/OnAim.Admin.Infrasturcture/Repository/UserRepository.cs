using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Exceptions;
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
        private readonly IRoleRepository _roleRepository;

        public UserRepository(
            DatabaseContext databaseContext, 
            IRoleRepository roleRepository
            )
        {
            _databaseContext = databaseContext;
            _roleRepository = roleRepository;
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
                IsBanned = user.IsBanned,
                IsActive = true,
                UserId = user.UserId,
                DateCreated = user.DateCreated,
            };
            _databaseContext.Users.Add(res);
            await _databaseContext.SaveChangesAsync();

            var role = await _roleRepository.GetRoleByName("DefaultRole");
            await _roleRepository.AssignRoleToUserAsync(res.Id, role.Id);

            return res;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _databaseContext.Users
                .AsNoTracking()
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
                          .ThenInclude(x => x.RoleEndpointGroups)
                          .ThenInclude(x => x.EndpointGroup)
                          .ThenInclude(x => x.EndpointGroupEndpoints)
                          .ThenInclude(x => x.Endpoint)
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

            var users = await query
                .OrderBy(x => x.Id)
                .Skip((userFilter.PageNumber - 1) * userFilter.PageSize)
                .Take(userFilter.PageSize)
                .ToListAsync();

            var result = users
                .Select(user => new UsersResponseModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
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
                                Type = ToHttpMethod(u.Endpoint.Type),
                                Path = u.Endpoint.Path,
                                Description = u.Endpoint.Description,
                            }).ToList()
                        }).ToList(),
                    }).ToList()
                })
                .ToList();

            return new PaginatedResult<UsersResponseModel>
            {
                PageNumber = userFilter.PageNumber,
                PageSize = userFilter.PageSize,
                TotalCount = totalCount,
                Items = result
            };
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _databaseContext.FindAsync<User>(userId);

            if (user != null)
            {
                user.IsActive = false;
                await _databaseContext.SaveChangesAsync();
            }
        }

        public static string ToHttpMethod(EndpointType? type)
        {
            return type switch
            {
                EndpointType.Get => "GET",
                EndpointType.Create => "POST",
                EndpointType.Update => "PUT",
                EndpointType.Delete => "DELETE",
                _ => "UNKNOWN"
            };
        }

        public async Task<User> UpdateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var existingUser = await _databaseContext.Users.FindAsync(user.Id);

            if (existingUser == null)
            {
                throw new UserNotFoundException("User not found");
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.Salt = user.Salt;
            existingUser.Password = user.Password; 
            existingUser.Phone = user.Phone;
            existingUser.IsBanned = user.IsBanned;
            existingUser.UserId = user.UserId; 

            await _databaseContext.SaveChangesAsync();

            return existingUser;
        }

        public async Task CommitChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
