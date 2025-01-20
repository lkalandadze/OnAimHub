using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class ServiceRepository(HubDbContext context) : BaseRepository<HubDbContext, Service>(context), IServiceRepository
{
    public async Task<int> InsertAndSaveAsync(Service aggregateRoot)
    {
        await _context.Services.AddAsync(aggregateRoot);
        await _context.SaveChangesAsync();

        return (int)typeof(Service).GetProperty(nameof(Service.Id))?.GetValue(aggregateRoot)!;
    }
}