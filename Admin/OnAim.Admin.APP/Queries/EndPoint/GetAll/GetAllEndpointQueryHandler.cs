using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Endpoint;
using OnAim.Admin.Shared.Helpers;
using OnAim.Admin.Shared.Paging;

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

            var paginatedResult = await Paginator.GetPaginatedResult(
                query,
                request.Filter,
                item => new EndpointResponseModel
                {
                    Id = item.Id,
                    Path = item.Path,
                    Name = item.Name,
                    Description = item.Description,
                    Type = ToHttpMethodExtension.ToHttpMethod(item.Type),
                    IsActive = item.IsActive,
                    IsDeleted = item.IsDeleted,
                    DateCreated = item.DateCreated,
                    DateUpdated = item.DateUpdated,
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
