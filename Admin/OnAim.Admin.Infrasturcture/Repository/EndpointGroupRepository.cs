using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Persistance.Data;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.Infrasturcture.Repository
{
    public class EndpointGroupRepository : IEndpointGroupRepository
    {
        private readonly DatabaseContext _databaseContext;

        public EndpointGroupRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task AddAsync(EndpointGroup endpointGroup)
        {
            await _databaseContext.EndpointGroups.AddAsync(endpointGroup);
        }

        public async Task AddEndpoint(EndpointGroup endpointGroup, Endpoint endpoint)
        {
            var exists = await _databaseContext.EndpointGroupEndpoints
                                   .AnyAsync(ege => ege.EndpointGroupId == endpointGroup.Id && ege.EndpointId == endpoint.Id);

            if (!exists)
            {
                var endpointGroupEndpoint = new EndpointGroupEndpoint
                {
                    EndpointGroupId = endpointGroup.Id,
                    EndpointId = endpoint.Id,
                    Endpoint = endpoint,
                    EndpointGroup = endpointGroup
                };

                _databaseContext.EndpointGroupEndpoints.Add(endpointGroupEndpoint);
            }

        }

        public async Task DeleteAsync(int id)
        {
            var endpointGroup = await GetByIdAsync(id);
            if (endpointGroup != null)
            {
                _databaseContext.EndpointGroups.Remove(endpointGroup);
            }
        }

        public async Task<IEnumerable<EndpointGroup>> GetAllAsync()
        {
            return await _databaseContext.EndpointGroups
                                       .AsNoTracking()
                                       .Include(x => x.EndpointGroupEndpoints)
                                       .ThenInclude(x => x.Endpoint)
                                       .ToListAsync();
        }

        public async Task<EndpointGroup> GetByIdAsync(int id)
        {
            return await _databaseContext.EndpointGroups
                                         .Include(x => x.EndpointGroupEndpoints)
                                         .ThenInclude(x => x.Endpoint)
                                         .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<EndpointGroup> GetEndpointGroupByRole(int roleId)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveEndpoint(EndpointGroup endpointGroup, Endpoint endpoint)
        {
            var endpointGroupEndpoint = await _databaseContext.EndpointGroupEndpoints
                                               .FirstOrDefaultAsync(ege => ege.EndpointGroupId == endpointGroup.Id && ege.EndpointId == endpoint.Id);

            if (endpointGroupEndpoint != null)
            {
                _databaseContext.EndpointGroupEndpoints.Remove(endpointGroupEndpoint);
            }

        }

        public async Task SaveChangesAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(EndpointGroup endpointGroup)
        {
            _databaseContext.EndpointGroups.Update(endpointGroup);
        }
    }
}
