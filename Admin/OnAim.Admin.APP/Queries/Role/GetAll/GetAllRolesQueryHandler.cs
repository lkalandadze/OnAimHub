using MediatR;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Infrasturcture.Models.Response;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Models.Response.Role;

namespace OnAim.Admin.APP.Queries.Role.GetAll
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, ApplicationResult>
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
                          &&
               x.Name != "SuperRole"
                     );

            var totalCount = await roleQuery.CountAsync(cancellationToken);

            var pageNumber = request.Filter.PageNumber ?? 1;
            var pageSize = request.Filter.PageSize ?? 25;

            bool sortDescending = request.Filter.SortDescending.GetValueOrDefault();

            if (request.Filter.SortBy == "Id")
            {
                roleQuery = sortDescending
                    ? roleQuery.OrderByDescending(x => x.Id)
                    : roleQuery.OrderBy(x => x.Id);
            }
            else if (request.Filter.SortBy == "Name")
            {
                roleQuery = sortDescending
                    ? roleQuery.OrderByDescending(x => x.Name)
                    : roleQuery.OrderBy(x => x.Name);
            }

            var roleResult = roleQuery
                .Select(x => new RoleResponseModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    EndpointGroupModels = x.RoleEndpointGroups
                        .Select(z => new EndpointGroupModeldTO
                        {
                            Id = z.EndpointGroup.Id,
                            Name = z.EndpointGroup.Name,
                            IsActive = z.EndpointGroup.IsActive,
                        }).ToList()
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var items = await roleResult.ToListAsync(cancellationToken);

            return new ApplicationResult
            {
                Success = true,
                Data = new PaginatedResult<RoleResponseModel>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Items = items
                }
            };
        }
    }
}
