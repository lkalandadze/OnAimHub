using Microsoft.EntityFrameworkCore;

namespace AggregationService.Infrastructure.Persistance.Data;

public class AggregationServiceContext : DbContext
{
    public AggregationServiceContext(DbContextOptions<AggregationServiceContext> options) : base(options)
    {
        
    }
}
