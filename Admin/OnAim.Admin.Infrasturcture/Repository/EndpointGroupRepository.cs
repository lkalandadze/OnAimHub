using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Infrasturcture.Models.Response;
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

        public async Task<PaginatedResult<EndpointGroup>> GetAllAsync(EndpointFilter filter)
        {
            var query = _databaseContext.EndpointGroups
                .AsNoTracking()
                .Include(x => x.EndpointGroupEndpoints)
                .ThenInclude(x => x.Endpoint).AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(x => x.Name.Contains(filter.Name));
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive);
            }

            if (filter.IsEnable.HasValue)
            {
                query = query.Where(x => x.IsEnabled == filter.IsEnable);
            }

            var totalCount = await query.CountAsync();

            var endpointGroups = await query
               .OrderBy(x => x.Id)
               .Skip((filter.PageNumber - 1) * filter.PageSize)
               .Take(filter.PageSize)
               .ToListAsync();

            var result = endpointGroups.Select(x => new EndpointGroup
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive,
                Description = x.Description,
                IsEnabled = x.IsEnabled,
                UserId = x.UserId,
                DateCreated = x.DateCreated,
                DateDeleted = x.DateDeleted,
                DateUpdated = x.DateUpdated,
                EndpointGroupEndpoints = x.EndpointGroupEndpoints.Select(ep => new EndpointGroupEndpoint
                {
                    EndpointId = ep.EndpointId,
                    EndpointGroupId = ep.EndpointGroupId,
                    Endpoint = new Endpoint
                    {
                        Id = ep.Endpoint.Id,
                        Name = ep.Endpoint.Name,
                        Path = ep.Endpoint.Path,
                        Description = ep.Endpoint.Description,
                        IsActive = ep.Endpoint.IsActive,
                        IsEnabled = ep.Endpoint.IsEnabled,
                        UserId = ep.Endpoint.UserId,
                        DateCreated = ep.Endpoint.DateCreated,
                        DateDeleted = ep.Endpoint.DateDeleted,
                        DateUpdated = ep.Endpoint.DateUpdated
                    }
                }).ToList()
            }).ToList();

            return new PaginatedResult<EndpointGroup>
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount,
                Items = result
            };
        }

        public async Task<EndpointGroup> GetByIdAsync(int id)
        {
            var group = await _databaseContext.EndpointGroups
                                         .Include(x => x.EndpointGroupEndpoints)
                                         .ThenInclude(x => x.Endpoint)
                                         .FirstOrDefaultAsync(x => x.Id == id);

            return new EndpointGroup
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                IsActive = group.IsActive,
                IsEnabled = group.IsActive,
                UserId = group.UserId,
                DateCreated = group.DateCreated,
                DateDeleted = group.DateDeleted,
                DateUpdated = group.DateUpdated,
                EndpointGroupEndpoints = group.EndpointGroupEndpoints.Select(x => new EndpointGroupEndpoint
                {
                    EndpointId = x.EndpointId,
                    EndpointGroupId = x.EndpointGroupId,
                    Endpoint = new Endpoint
                    {
                        Id = x.Endpoint.Id,
                        Name = x.Endpoint.Name,
                        Path = x.Endpoint.Path,
                        Description = x.Endpoint.Description,
                        IsActive = x.Endpoint.IsActive,
                        IsEnabled = x.Endpoint.IsEnabled,
                        UserId = x.Endpoint.UserId,
                        DateCreated = x.Endpoint.DateCreated,
                        DateDeleted = x.Endpoint.DateDeleted,
                        DateUpdated = x.Endpoint.DateUpdated
                    }
                }).ToList()
            };
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
