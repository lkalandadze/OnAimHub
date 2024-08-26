using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Exceptions;
using OnAim.Admin.Infrasturcture.Extensions;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Models.Response.Endpoint;
using OnAim.Admin.Infrasturcture.Persistance.Data;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.Models;
using System.Data;

namespace OnAim.Admin.Infrasturcture.Repository
{
    public class EndpointRepository : IEndpointRepository
    {
        private readonly DatabaseContext _databaseContext;

        public EndpointRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<PaginatedResult<EndpointResponseModel>> GetAllEndpoints(EndpointFilter filter)
        {
            var query = _databaseContext.Endpoints
                    .AsNoTracking()
                    .Where(x =>
                        (string.IsNullOrEmpty(filter.Name) || x.Name.Contains(filter.Name)) &&
                        (!filter.IsActive.HasValue || x.IsActive == filter.IsActive) &&
                        (!filter.Type.HasValue || x.Type == filter.Type.Value)
                    );

            var totalCount = await query.CountAsync();

            var pageNumber = filter.PageNumber ?? 1;
            var pageSize = filter.PageSize ?? 25;

            var endpoints = await query
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = endpoints.Select(ep => new EndpointResponseModel
            {
                Id = ep.Id,
                Name = ep.Name,
                Path = ep.Path,
                Description = ep.Description,
                IsEnabled = ep.IsEnabled,
                IsActive = ep.IsActive,
                Type = ToHttpMethodExtension.ToHttpMethod(ep.Type),
                UserId = ep.UserId,
                DateCreated = ep.DateCreated,
                DateUpdated = ep.DateUpdated,
            }).ToList();

            return new PaginatedResult<EndpointResponseModel>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = result
            };
        }

        public async Task<Endpoint> GetEndpointById(int id)
        {
            var endpoint = await _databaseContext.Endpoints.FirstOrDefaultAsync(x => x.Id == id);

            if (endpoint == null)
            {
                throw new EndpointNotFoundException("Endpoint Not Found");
            }

            return endpoint;
        }

        public async Task<Endpoint> CreateEndpointAsync(string name, string description = null, string? endpointType = null)
        {
            var endpoint = await _databaseContext.Endpoints.FirstOrDefaultAsync(e => e.Name == name);

            EndpointType endpointTypeEnum = EndpointType.Get;

            if (endpointType != null && Enum.TryParse(endpointType, true, out EndpointType parsedType))
            {
                endpointTypeEnum = parsedType;
            }

            if (endpoint != null)
            {
                throw new AlreadyExistsException("Endpoint with that name already exists.");
            }

            if (endpoint == null)
            {
                endpoint = new Endpoint
                {
                    Name = name,
                    Path = name,
                    Description = description ?? "Description needed",
                    IsEnabled = true,
                    DateCreated = SystemDate.Now,
                    //UserId = userId,
                    IsActive = true,
                    Type = endpointTypeEnum,
                };
                _databaseContext.Endpoints.Add(endpoint);
                await _databaseContext.SaveChangesAsync();
            }
            else if (string.IsNullOrEmpty(endpoint.Description))
            {
                endpoint.Description = "Description needed";
                _databaseContext.Endpoints.Update(endpoint);
                await _databaseContext.SaveChangesAsync();
            }

            return endpoint;
        }

        public async Task UpdateEndpoint(int id, UpdateEndpointDto model)
        {
            var ep = await GetEndpointById(id);

            if (ep != null)
            {
                ep.Name = model.Name;
                ep.Description = model.Description;
                ep.IsActive = model.IsActive ?? true;
                ep.IsEnabled = model.IsEnabled ?? true;
                ep.DateUpdated = SystemDate.Now;

                await _databaseContext.SaveChangesAsync();
            }
        }
    }
}
