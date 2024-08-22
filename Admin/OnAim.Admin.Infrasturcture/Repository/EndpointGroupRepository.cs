using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Exceptions;
using OnAim.Admin.Infrasturcture.Models.Request.EndpointGroup;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Persistance.Data;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.Models;

namespace OnAim.Admin.Infrasturcture.Repository
{
    public class EndpointGroupRepository : IEndpointGroupRepository
    {
        private readonly DatabaseContext _databaseContext;

        public EndpointGroupRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task AddAsync(CreateEndpointGroupRequest model)
        {
            var existedGroupName = await _databaseContext.EndpointGroups.Where(x => x.Name == model.Name).FirstOrDefaultAsync();

            if (existedGroupName == null)
            {
                var endpointGroup = new EndpointGroup
                {
                    Name = model.Name,
                    Description = model.Description,
                    IsEnabled = true,
                    IsActive = true,
                    EndpointGroupEndpoints = new List<EndpointGroupEndpoint>(),
                    //UserId = ,
                    DateCreated = SystemDate.Now,
                };

                foreach (var endpointId in model.EndpointIds)
                {
                    var endpoint = await _databaseContext.Endpoints.FindAsync(endpointId);

                    if (!endpoint.IsEnabled)
                    {
                        throw new Exception("Endpoint Is Disabled!");
                    }

                    var endpointGroupEndpoint = new EndpointGroupEndpoint
                    {
                        Endpoint = endpoint,
                        EndpointGroup = endpointGroup
                    };

                    endpointGroup.EndpointGroupEndpoints.Add(endpointGroupEndpoint);
                }

                await _databaseContext.EndpointGroups.AddAsync(endpointGroup);
                await SaveChangesAsync();
            }
            else
            {
                throw new AlreadyExistsException("Group with that name already exists!");
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

        public async Task<PaginatedResult<EndpointGroup>> GetAllAsync(EndpointGroupFilter filter)
        {
            var query = _databaseContext.EndpointGroups
                .AsNoTracking()
                .Include(x => x.EndpointGroupEndpoints)
                .ThenInclude(x => x.Endpoint)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(x => x.Name.Contains(filter.Name));
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == filter.IsActive);
            }

            var totalCount = await query.CountAsync();

            var pageNumber = filter.PageNumber ?? 1;
            var pageSize = filter.PageSize ?? 25;

            var endpointGroups = await query
               .OrderBy(x => x.Id)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
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
                }).ToList()
            }).ToList();

            return new PaginatedResult<EndpointGroup>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
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

        public async Task RemoveEndpoint(EndpointGroup endpointGroup, Endpoint endpoint)
        {
            var endpointGroupEndpoint = await _databaseContext.EndpointGroupEndpoints
                                               .FirstOrDefaultAsync(ege => ege.EndpointGroupId == endpointGroup.Id && ege.EndpointId == endpoint.Id);

            if (endpointGroupEndpoint != null)
            {
                _databaseContext.EndpointGroupEndpoints.Remove(endpointGroupEndpoint);
            }

        }

        public async Task UpdateAsync(int id, UpdateEndpointGroupRequest endpointGroup)
        {
            var group = await _databaseContext.EndpointGroups
                .Include(g => g.EndpointGroupEndpoints)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
            {
                throw new Exception("Endpoint Group Not Found");
            }

            var currentEndpointIds = group.EndpointGroupEndpoints.Select(ep => ep.EndpointId).ToList();
            var newEndpointIds = endpointGroup.EndpointIds ?? new List<int>();

            var endpointsToAdd = newEndpointIds.Except(currentEndpointIds).ToList();
            var endpointsToRemove = currentEndpointIds.Except(newEndpointIds).ToList();

            foreach (var endpointId in endpointsToAdd)
            {
                var endpoint = await _databaseContext.Endpoints.FindAsync(endpointId);

                if (endpoint != null)
                {
                    var alreadyExists = group.EndpointGroupEndpoints.Any(ege => ege.EndpointId == endpointId);
                    if (!alreadyExists)
                    {
                        _databaseContext.EndpointGroupEndpoints.Add(new EndpointGroupEndpoint
                        {
                            EndpointGroupId = group.Id,
                            EndpointId = endpoint.Id,
                            Endpoint = endpoint,
                            EndpointGroup = group
                        });
                    }
                }
            }

            foreach (var endpointId in endpointsToRemove)
            {
                var endpointGroupEndpoint = await _databaseContext.EndpointGroupEndpoints
                    .FirstOrDefaultAsync(ege => ege.EndpointGroupId == group.Id && ege.EndpointId == endpointId);

                if (endpointGroupEndpoint != null)
                {
                    _databaseContext.EndpointGroupEndpoints.Remove(endpointGroupEndpoint);
                }
            }

            group.Name = endpointGroup.Name;
            group.Description = endpointGroup.Description;
            group.IsActive = endpointGroup.IsActive ?? true;

            await _databaseContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
