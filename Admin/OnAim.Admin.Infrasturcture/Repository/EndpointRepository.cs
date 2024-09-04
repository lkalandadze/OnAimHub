using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Extensions;
using OnAim.Admin.Infrasturcture.Models.Request.Endpoint;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Models.Response.Endpoint;
using OnAim.Admin.Infrasturcture.Persistance.Data;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
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
                IsEnabled = ep.IsDeleted,
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
    }
}
