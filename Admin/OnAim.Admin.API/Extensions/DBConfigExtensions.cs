using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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

            if (!await dbContext.EndpointGroups.AnyAsync())
            {
                var endpointGroup = new EndpointGroup
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SuperGroup",
                    Description = "All Permission for super admin",
                    IsEnabled = true,
                    IsActive = true,
                    EndpointGroupEndpoints = new List<EndpointGroupEndpoint>(),
                    DateCreated = DateTime.UtcNow
                };

                foreach (var item in dbContext.Endpoints)
                {
                    var endpointGroupEndpoint = new EndpointGroupEndpoint
                    {
                        EndpointId = item.Id,
                        EndpointGroupId = endpointGroup.Id,
                    };

                    endpointGroup.EndpointGroupEndpoints.Add(endpointGroupEndpoint);
                    await dbContext.SaveChangesAsync();
                }
                dbContext.EndpointGroups.Add(endpointGroup);
                await dbContext.SaveChangesAsync();
            }

            if (!await dbContext.Roles.AnyAsync())
            {
                var rolee = new Role
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SuperRole",
                    Description = "role for super admin",
                    RoleEndpointGroups = new List<RoleEndpointGroup>()
                };
                dbContext.Roles.Add(rolee);

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
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "SuperAdmin",
                    LastName = "SuperAdmin",
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
