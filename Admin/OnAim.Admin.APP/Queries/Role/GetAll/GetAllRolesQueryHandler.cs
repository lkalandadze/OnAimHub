using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Shared.Paging;
using OnAim.Admin.Shared.DTOs.Role;
using OnAim.Admin.Shared.DTOs.EndpointGroup;

namespace OnAim.Admin.APP.Queries.Role.GetAll
{
    public class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.Role> _repository;

        public GetAllRolesQueryHandler(IRepository<Infrasturcture.Entities.Role> repository)
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

            if (request.Filter.UserIds != null && request.Filter.UserIds.Any())
                roleQuery = roleQuery.Where(x => x.UserRoles.Any(ur => request.Filter.UserIds.Contains(ur.UserId)));


            if (request.Filter.GroupIds != null && request.Filter.GroupIds.Any())
                roleQuery = roleQuery.Where(x => x.RoleEndpointGroups.Any(ur => request.Filter.GroupIds.Contains(ur.EndpointGroupId)));

            if (request.Filter.IsDeleted.HasValue)
                roleQuery = roleQuery.Where(x => x.IsDeleted == request.Filter.IsDeleted);

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
}
