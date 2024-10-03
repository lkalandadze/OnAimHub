using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.API.Service.Endpoint;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Persistance.Data.Admin;
using System.Security.Cryptography;

namespace OnAim.Admin.API.Extensions;

public static class DBConfigExtensions
{
    public static async Task SeedDatabaseAsync(this DatabaseContext dbContext)
    {
        await SeedEndpointsAsync(dbContext);

        if (!await dbContext.EndpointGroups.AnyAsync(x => x.Name == "SuperGroup"))
        {
            var endpointGroup = EndpointGroup.Create("SuperGroup", "All Permission for super admin", null, new List<EndpointGroupEndpoint>());

            dbContext.EndpointGroups.Add(endpointGroup);

            await dbContext.SaveChangesAsync();

            foreach (var item in dbContext.Endpoints)
            {
                var endpointGroupEndpoint = new EndpointGroupEndpoint(endpointGroup.Id, item.Id);

                endpointGroup.EndpointGroupEndpoints.Add(endpointGroupEndpoint);
            }

            await dbContext.SaveChangesAsync();
        }

        if (!await dbContext.Roles.AnyAsync(x => x.Name == "SuperRole"))
        {
            var rolee = new Role("SuperRole", "role for super admin", null);

            dbContext.Roles.Add(rolee);

            await dbContext.SaveChangesAsync();

            var endpointGroup = await dbContext.EndpointGroups.FirstOrDefaultAsync(x => x.Name == "SuperGroup");
            if (endpointGroup != null)
            {
                var roleEndpointGroup = new RoleEndpointGroup(rolee.Id, endpointGroup.Id);

                dbContext.RoleEndpointGroups.Add(roleEndpointGroup);
            }

            await dbContext.SaveChangesAsync();
        }

        var defaultGroup = await dbContext.EndpointGroups.FirstOrDefaultAsync(x => x.Name == "DefaultGroup");
        if (defaultGroup == null)
        {
            defaultGroup = EndpointGroup.Create("DefaultGroup", "Default permission group", null, new List<EndpointGroupEndpoint>());

            dbContext.EndpointGroups.Add(defaultGroup);
            await dbContext.SaveChangesAsync();

            var usersGetMeEndpoint = await dbContext.Endpoints.FirstOrDefaultAsync(x => x.Name == "GetMe_Users");
            var defaultendpointGroupEndpoint = new EndpointGroupEndpoint(defaultGroup.Id, usersGetMeEndpoint.Id);

            defaultGroup.EndpointGroupEndpoints.Add(defaultendpointGroupEndpoint);

            var usersProfileUpdateEndpoint = await dbContext.Endpoints.FirstOrDefaultAsync(x => x.Name == "ProfileUpdate_Users");
            var defaultProfileUpdateEndpointGroupEndpoint = new EndpointGroupEndpoint(defaultGroup.Id, usersProfileUpdateEndpoint.Id);

            defaultGroup.EndpointGroupEndpoints.Add(defaultProfileUpdateEndpointGroupEndpoint);

            await dbContext.SaveChangesAsync();
        }

        if (!await dbContext.Roles.AnyAsync(x => x.Name == "DefaultRole"))
        {
            var newRole = new Role("DefaultRole", "Role with DefaultGroup and Users_GetMe", null);

            dbContext.Roles.Add(newRole);
            await dbContext.SaveChangesAsync();

            var roleEndpointGroup = await dbContext.EndpointGroups.FirstOrDefaultAsync(x => x.Name == "DefaultGroup");
            if (roleEndpointGroup != null)
            {
                var defaultRoleEndpointGroup = new RoleEndpointGroup(newRole.Id, roleEndpointGroup.Id);

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

            dbContext.Users.Add(new User(
                "SuperAdmin", 
                "SuperAdmin", 
                "superadmin", 
                "superadmin@test.com",
                hashedPassword,
                salt,
                "595999999",
                null, 
                true, 
                true,
                null,
                null,
                true));

            await dbContext.SaveChangesAsync();
        }

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == "superadmin@test.com");
        var role = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "SuperRole");

        if (user != null && role != null)
        {
            if (!await dbContext.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id))
            {
                var userRole = new UserRole(user.Id, role.Id);

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
