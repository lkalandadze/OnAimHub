using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hub.Api.Db
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }
    }

    public class ApplicationUser : IdentityUser
    {
    }
}
