using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using System.Data;

namespace OnAim.Admin.Infrasturcture.Persistance.Data
{
    public class DatabaseContext : DbContext, IUnitOfWork
    {
        private IDbContextTransaction _dbContextTransaction;

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; }
        public DbSet<EndpointGroup> EndpointGroups { get; set; }
        public DbSet<Endpoint> Endpoints { get; set; }
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<RoleEndpointGroup> RoleEndpointGroups { get; set; } = null!;
        public DbSet<EndpointGroupEndpoint> EndpointGroupEndpoints { get; set; } = null!;

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<RejectedLog> RejectedLogs { get; set; }
        public DbSet<AllowedEmailDomain> AllowedDomains { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }

        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                  .HasMany(u => u.AccessTokens)
                  .WithOne(at => at.User)
                  .HasForeignKey(at => at.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId);

            modelBuilder.Entity<AccessToken>()
                .HasIndex(at => at.Token)
                .IsUnique();

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(rt => rt.Token)
                .IsUnique();

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

            modelBuilder.Entity<AppSetting>()
               .ToTable("AppSettings")
               .HasKey(a => a.Id);

            modelBuilder.Entity<AppSetting>()
                .Property(a => a.Key)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<AppSetting>()
                .Property(a => a.Value)
                .IsRequired();
        }

        public async Task<IDisposable> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        {
            _dbContextTransaction = await Database.BeginTransactionAsync(isolationLevel, cancellationToken);
            return _dbContextTransaction;
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _dbContextTransaction.CommitAsync(cancellationToken);
        }
    }


}
