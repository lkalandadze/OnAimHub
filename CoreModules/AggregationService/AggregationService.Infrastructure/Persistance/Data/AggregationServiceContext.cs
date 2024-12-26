using AggregationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AggregationService.Infrastructure.Persistance.Data;

public class AggregationServiceContext : DbContext
{
    public AggregationServiceContext(DbContextOptions<AggregationServiceContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

    public DbSet<Aggregation> Aggregations { get; set; }
    public DbSet<AggregationConfiguration> AggregationConfigurations { get; set; }

}