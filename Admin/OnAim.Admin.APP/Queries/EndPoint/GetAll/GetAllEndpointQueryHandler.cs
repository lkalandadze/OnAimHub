using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Endpoint;
using OnAim.Admin.Shared.Helpers;
using OnAim.Admin.Shared.Paging;
using System.Linq.Expressions;

namespace OnAim.Admin.APP.Queries.EndPoint.GetAll
{
    public class GetAllEndpointQueryHandler : IQueryHandler<GetAllEndpointQuery, ApplicationResult>
    {
        private readonly IRepository<Endpoint> _repository;

        public GetAllEndpointQueryHandler(IRepository<Endpoint> repository)
        {
            _repository = repository;
        }
        public async Task<ApplicationResult> Handle(GetAllEndpointQuery request, CancellationToken cancellationToken)
        {
            var query = _repository
                .Query(x =>
                         (string.IsNullOrEmpty(request.Filter.Name) || x.Name.Contains(request.Filter.Name)) &&
                         (!request.Filter.IsActive.HasValue || x.IsActive == request.Filter.IsActive.Value) &&
                         (!request.Filter.Type.HasValue || x.Type == request.Filter.Type.Value)
                     );

            if (request.Filter.EndpointGroupIds != null && request.Filter.EndpointGroupIds.Any())
                query = query.Where(x => x.EndpointGroupEndpoints.Any(ur => request.Filter.EndpointGroupIds.Contains(ur.EndpointGroupId)));

            if (request.Filter.IsDeleted.HasValue)
                query = query.Where(x => x.IsDeleted == request.Filter.IsDeleted);

            if (!request.Filter.IsActive.HasValue && !request.Filter.IsDeleted.HasValue)
            {
                // No filtering on active/deleted status
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var pageNumber = request.Filter.PageNumber ?? 1;
            var pageSize = request.Filter.PageSize ?? 25;

            var sortDescending = request.Filter.SortDescending.GetValueOrDefault();
            var sortBy = request.Filter.SortBy?.ToLower();

            Expression<Func<Infrasturcture.Entities.Endpoint, object>> sortExpression = sortBy switch
            {
                "id" => x => x.Id,
                "name" => x => x.Name,
                _ => x => x.Id
            };

            query = sortDescending ? query.OrderByDescending(sortExpression) : query.OrderBy(sortExpression);

            var endpoints = query
                .Select(x => new EndpointResponseModel
                {
                    Id = x.Id,
                    Path = x.Path,
                    Name = x.Name,
                    Description = x.Description,
                    Type = ToHttpMethodExtension.ToHttpMethod(x.Type),
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return new ApplicationResult
            {
                Success = true,
                Data = new PaginatedResult<EndpointResponseModel>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Items = await endpoints.ToListAsync()
                }
            };
        }
    }
}
