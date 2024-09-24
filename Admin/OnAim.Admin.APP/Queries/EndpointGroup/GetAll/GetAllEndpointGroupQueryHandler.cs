using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Endpoint;
using OnAim.Admin.Shared.DTOs.EndpointGroup;
using OnAim.Admin.Shared.Helpers;
using OnAim.Admin.Shared.Paging;

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

            var paginatedResult = await Paginator.GetPaginatedResult(
                query,
                request.Filter,
                item => new EndpointGroupModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    DateUpdated = item.DateUpdated,
                    DateCreated = item.DateCreated,
                    DateDeleted = item.DateDeleted,
                    EndpointsCount = item.EndpointGroupEndpoints.Count,
                    Endpoints = item.EndpointGroupEndpoints.Select(xx => new EndpointRequestModel
                    {
                        Id = xx.Endpoint.Id,
                        Name = xx.Endpoint.Name,
                        Path = xx.Endpoint.Path,
                        Description = xx.Endpoint.Description,
                        Type = ToHttpMethodExtension.ToHttpMethod(xx.Endpoint.Type),
                        IsActive = xx.Endpoint.IsActive,
                        DateCreated = xx.Endpoint.DateCreated,
                    }).ToList(),
                    IsActive = item.IsActive,
                    IsDeleted = item.IsDeleted,
                },
                cancellationToken
            );


            return new ApplicationResult
            {
                Success = true,
                Data = paginatedResult
            };
        }
    }
}
