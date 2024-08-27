using Hub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hub.Infrastructure.DataAccess;

public class HubDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}