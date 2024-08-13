using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;

namespace OnAim.Admin.Infrasturcture.Persistance.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; }
        public DbSet<EndpointGroup> EndpointGroups { get; set; }
        public DbSet<Endpoint> Endpoints { get; set; }
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<RoleEndpointGroup> RoleEndpointGroups { get; set; } = null!;
        public DbSet<EndpointGroupEndpoint> EndpointGroupEndpoints { get; set; } = null!;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<RoleEndpointGroup>()
                .HasKey(reg => new { reg.RoleId, reg.EndpointGroupId });
            modelBuilder.Entity<RoleEndpointGroup>()
                .HasOne(reg => reg.Role)
                .WithMany(r => r.RoleEndpointGroups)
                .HasForeignKey(reg => reg.RoleId);
            modelBuilder.Entity<RoleEndpointGroup>()
                .HasOne(reg => reg.EndpointGroup)
                .WithMany(eg => eg.RoleEndpointGroups)
                .HasForeignKey(reg => reg.EndpointGroupId);

            modelBuilder.Entity<EndpointGroupEndpoint>()
                .HasKey(ege => new { ege.EndpointGroupId, ege.EndpointId });
            modelBuilder.Entity<EndpointGroupEndpoint>()
                .HasOne(ege => ege.EndpointGroup)
                .WithMany(eg => eg.EndpointGroupEndpoints)
                .HasForeignKey(ege => ege.EndpointGroupId);
            modelBuilder.Entity<EndpointGroupEndpoint>()
                .HasOne(ege => ege.Endpoint)
                .WithMany(e => e.EndpointGroupEndpoints)
                .HasForeignKey(ege => ege.EndpointId);
        }
    }


}
