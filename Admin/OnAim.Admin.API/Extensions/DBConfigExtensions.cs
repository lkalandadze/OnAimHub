using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.API.Service;
using OnAim.Admin.API.Service.Endpoint;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Persistance.Data;
using System.Security.Cryptography;

namespace OnAim.Admin.API.Extensions
{
    public static class DBConfigExtensions
    {
        public static async Task SeedDatabaseAsync(this DatabaseContext dbContext)
        {
            await SeedEndpointsAsync(dbContext);

            if (!await dbContext.EndpointGroups.AnyAsync(x => x.Name == "SuperGroup"))
            {
                var endpointGroup = new EndpointGroup
                {
                    Name = "SuperGroup",
                    Description = "All Permission for super admin",
                    IsEnabled = true,
                    IsActive = true,
                    EndpointGroupEndpoints = new List<EndpointGroupEndpoint>(),
                    DateCreated = DateTime.UtcNow
                };

                dbContext.EndpointGroups.Add(endpointGroup);

                await dbContext.SaveChangesAsync();

                foreach (var item in dbContext.Endpoints)
                {
                    var endpointGroupEndpoint = new EndpointGroupEndpoint
                    {
                        EndpointId = item.Id,
                        EndpointGroupId = endpointGroup.Id,
                    };

                    endpointGroup.EndpointGroupEndpoints.Add(endpointGroupEndpoint);
                }

                await dbContext.SaveChangesAsync();
            }

            if (!await dbContext.Roles.AnyAsync(x => x.Name == "SuperRole"))
            {
                var rolee = new Role
                {
                    Name = "SuperRole",
                    Description = "role for super admin",
                    IsActive = true,
                    RoleEndpointGroups = new List<RoleEndpointGroup>()
                };

                dbContext.Roles.Add(rolee);

                await dbContext.SaveChangesAsync();

                var endpointGroup = await dbContext.EndpointGroups.FirstOrDefaultAsync(x => x.Name == "SuperGroup");
                if (endpointGroup != null)
                {
                    var roleEndpointGroup = new RoleEndpointGroup
                    {
                        RoleId = rolee.Id,
                        EndpointGroupId = endpointGroup.Id
                    };

                    dbContext.RoleEndpointGroups.Add(roleEndpointGroup);
                }

                await dbContext.SaveChangesAsync();
            }

            var defaultGroup = await dbContext.EndpointGroups.FirstOrDefaultAsync(x => x.Name == "DefaultGroup");
            if (defaultGroup == null)
            {
                defaultGroup = new EndpointGroup
                {
                    Name = "DefaultGroup",
                    Description = "Default permission group",
                    IsEnabled = true,
                    IsActive = true,
                    EndpointGroupEndpoints = new List<EndpointGroupEndpoint>(),
                    DateCreated = DateTime.UtcNow
                };

                dbContext.EndpointGroups.Add(defaultGroup);
                await dbContext.SaveChangesAsync();

                var usersGetMeEndpoint = await dbContext.Endpoints.FirstOrDefaultAsync(x => x.Name == "Users_GetMe");
                var defaultendpointGroupEndpoint = new EndpointGroupEndpoint
                {
                    EndpointId = usersGetMeEndpoint.Id,
                    EndpointGroupId = defaultGroup.Id
                };

                defaultGroup.EndpointGroupEndpoints.Add(defaultendpointGroupEndpoint);

                await dbContext.SaveChangesAsync();
            }

            if (!await dbContext.Roles.AnyAsync(x => x.Name == "DefaultRole"))
            {
                var newRole = new Role
                {
                    Name = "DefaultRole",
                    Description = "Role with DefaultGroup and Users_GetMe",
                    IsActive = true,
                    RoleEndpointGroups = new List<RoleEndpointGroup>()
                };

                dbContext.Roles.Add(newRole);
                await dbContext.SaveChangesAsync();

                var roleEndpointGroup = await dbContext.EndpointGroups.FirstOrDefaultAsync(x => x.Name == "DefaultGroup");
                if (roleEndpointGroup != null) 
                {
                    var defaultRoleEndpointGroup = new RoleEndpointGroup
                    {
                        RoleId = newRole.Id,
                        EndpointGroupId = roleEndpointGroup.Id
                    };
                    dbContext.RoleEndpointGroups.Add(defaultRoleEndpointGroup);
                }
                await dbContext.SaveChangesAsync();             
            }

            if (!await dbContext.Users.AnyAsync())
            {
                byte[] saltBytes = new byte[16];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(saltBytes);
                }
                string salt = Convert.ToBase64String(saltBytes);

                string hashedPassword = EncryptPassword("superadmin", saltBytes);

                dbContext.Users.Add(new User
                {
                    FirstName = "SuperAdmin",
                    LastName = "SuperAdmin",
                    Username = "superadmin",
                    Email = "superadmin@test.com",
                    Password = hashedPassword,
                    Salt = salt,
                    Phone = "595999999",
                    IsActive = true,
                    DateCreated = DateTime.UtcNow
                });

                await dbContext.SaveChangesAsync();
            }

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == "superadmin@test.com");
            var role = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "SuperRole");

            if (user != null && role != null)
            {
                if (!await dbContext.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id))
                {
                    var userRole = new UserRole
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    };

                    dbContext.UserRoles.Add(userRole);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
        private static async Task SeedEndpointsAsync(DatabaseContext dbContext)
        {
            var endpointService = new EndpointService(dbContext);

            await endpointService.SaveEndpointsAsync();
        }
        private static string EncryptPassword(string password, byte[] saltBytes)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        }
    }
}
