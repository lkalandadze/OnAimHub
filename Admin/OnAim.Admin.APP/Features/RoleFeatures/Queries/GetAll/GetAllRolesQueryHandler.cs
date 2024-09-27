using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.Paging;
using OnAim.Admin.Shared.DTOs.Role;
using OnAim.Admin.Shared.DTOs.EndpointGroup;
using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.Enums;

namespace OnAim.Admin.APP.Features.RoleFeatures.Queries.GetAll;

public class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, ApplicationResult>
{
    private readonly IRepository<Domain.Entities.Role> _repository;

    public GetAllRolesQueryHandler(IRepository<Domain.Entities.Role> repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationResult> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roleQuery = _repository
            .Query(x =>
                     (string.IsNullOrEmpty(request.Filter.Name) || x.Name.Contains(request.Filter.Name)) &&
                     (!request.Filter.IsActive.HasValue || x.IsActive == request.Filter.IsActive.Value)
                 );

        if (request.Filter?.HistoryStatus.HasValue == true)
        {
            switch (request.Filter.HistoryStatus.Value)
            {
                case HistoryStatus.Existing:
                    roleQuery = roleQuery.Where(u => u.IsDeleted == false);
                    break;
                case HistoryStatus.Deleted:
                    roleQuery = roleQuery.Where(u => u.IsDeleted == true);
                    break;
                case HistoryStatus.All:
                    break;
                default:
                    break;
            }
        }

        if (request.Filter.UserIds != null && request.Filter.UserIds.Any())
            roleQuery = roleQuery.Where(x => x.UserRoles.Any(ur => request.Filter.UserIds.Contains(ur.UserId)));

        if (request.Filter.GroupIds != null && request.Filter.GroupIds.Any())
            roleQuery = roleQuery.Where(x => x.RoleEndpointGroups.Any(ur => request.Filter.GroupIds.Contains(ur.EndpointGroupId)));

        var paginatedResult = await Paginator.GetPaginatedResult(
           roleQuery,
           request.Filter,
           role => new RoleShortResponseModel
           {
               Id = role.Id,
               Name = role.Name,
               Description = role.Description,
               IsActive = role.IsActive,
               IsDeleted = role.IsDeleted,
               EndpointGroupModels = role.RoleEndpointGroups
                    .Select(z => new EndpointGroupModeldTO
                    {
                        Id = z.EndpointGroup.Id,
                        Name = z.EndpointGroup.Name,
                        IsActive = z.EndpointGroup.IsActive,
                    }).ToList()
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
