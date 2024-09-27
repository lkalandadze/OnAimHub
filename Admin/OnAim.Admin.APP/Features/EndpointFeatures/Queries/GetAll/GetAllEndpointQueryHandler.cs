using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Domain.Entities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Endpoint;
using OnAim.Admin.Shared.Enums;
using OnAim.Admin.Shared.Helpers;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Queries.GetAll;

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

        if (request.Filter?.HistoryStatus.HasValue == true)
        {
            switch (request.Filter.HistoryStatus.Value)
            {
                case HistoryStatus.Existing:
                    query = query.Where(u => u.IsDeleted == false);
                    break;
                case HistoryStatus.Deleted:
                    query = query.Where(u => u.IsDeleted == true);
                    break;
                case HistoryStatus.All:
                    break;
                default:
                    break;
            }
        }

        if (request.Filter.GroupIds != null && request.Filter.GroupIds.Any())
            query = query.Where(x => x.EndpointGroupEndpoints.Any(ur => request.Filter.GroupIds.Contains(ur.EndpointGroupId)));

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
