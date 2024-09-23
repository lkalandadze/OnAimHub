using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Endpoint;
using OnAim.Admin.Shared.DTOs.EndpointGroup;
using OnAim.Admin.Shared.Helpers;
using OnAim.Admin.Shared.Paging;
using System.Linq.Expressions;

namespace OnAim.Admin.APP.Queries.EndpointGroup.GetAll
{
    public class GetAllEndpointGroupQueryHandler : IQueryHandler<GetAllEndpointGroupQuery, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.EndpointGroup> _repository;

        public GetAllEndpointGroupQueryHandler(IRepository<Infrasturcture.Entities.EndpointGroup> repository)
        {
            _repository = repository;
        }
        public async Task<ApplicationResult> Handle(GetAllEndpointGroupQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.Query(x =>
                         (string.IsNullOrEmpty(request.Filter.Name) || x.Name.Contains(request.Filter.Name)) &&
                         (!request.Filter.IsActive.HasValue || x.IsActive == request.Filter.IsActive.Value)
                );

            if (request.Filter.RoleIds != null && request.Filter.RoleIds.Any())
                query = query.Where(x => x.RoleEndpointGroups.Any(ur => request.Filter.RoleIds.Contains(ur.RoleId)));

            if (request.Filter.EndpointIds != null && request.Filter.EndpointIds.Any())
                query = query.Where(x => x.EndpointGroupEndpoints.Any(ur => request.Filter.EndpointIds.Contains(ur.EndpointId)));

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

            Expression<Func<Infrasturcture.Entities.EndpointGroup, object>> sortExpression = sortBy switch
            {
                "id" => x => x.Id,
                "name" => x => x.Name,
                _ => x => x.Id
            };

            query = sortDescending ? query.OrderByDescending(sortExpression) : query.OrderBy(sortExpression);

            var result = query
                .Select(x => new EndpointGroupModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    DateUpdated = x.DateUpdated,
                    DateCreated = x.DateCreated,
                    DateDeleted = x.DateDeleted,
                    EndpointsCount = x.EndpointGroupEndpoints.Count,
                    Endpoints = x.EndpointGroupEndpoints.Select(xx => new EndpointRequestModel
                    {
                        Id = xx.Endpoint.Id,
                        Name = xx.Endpoint.Name,
                        Path = xx.Endpoint.Path,
                        Description = xx.Endpoint.Description,
                        Type = ToHttpMethodExtension.ToHttpMethod(xx.Endpoint.Type),
                        IsActive = xx.Endpoint.IsActive,
                        DateCreated = xx.Endpoint.DateCreated,
                    }).ToList(),
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted,
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var rr = await result.ToListAsync();


            return new ApplicationResult
            {
                Success = true,
                Data = new PaginatedResult<EndpointGroupModel>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Items = await result.ToListAsync()
                }
            };
        }
    }
}
