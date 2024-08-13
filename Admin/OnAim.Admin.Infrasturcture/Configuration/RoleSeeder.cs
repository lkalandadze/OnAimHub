using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Persistance.Data;

namespace OnAim.Admin.Infrasturcture.Configuration
{
    public static class RoleSeeder
    {
        public static void SeedRoles(DatabaseContext context)
        {
            if (!context.Roles.Any())
            {
                var superAdminRole = new Role
                {
                    Id = "superadmin",
                    Name = "SuperAdmin",
                    Description = "Role with all permissions."
                };

                context.Roles.Add(superAdminRole);
                context.SaveChanges();

                var endpointGroups = context.EndpointGroups.ToList();
                foreach (var group in endpointGroups)
                {
                    var roleEndpointGroup = new RoleEndpointGroup
                    {
                        RoleId = superAdminRole.Id,
                        EndpointGroupId = group.Id
                    };
                    context.RoleEndpointGroups.Add(roleEndpointGroup);
                }
                context.SaveChanges();
            }
        }
    }
}
