using MediatR;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Models.Response.EndpointGroup;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.EndpointGroup.GetAll
{
    public class GetAllEndpointGroupQueryHandler : IRequestHandler<GetAllEndpointGroupQuery, ApplicationResult>
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
                         &&
               x.Name != "SuperGroup"
                );

            var totalCount = await query.CountAsync(cancellationToken);

            var pageNumber = request.Filter.PageNumber ?? 1;
            var pageSize = request.Filter.PageSize ?? 25;

            bool sortDescending = request.Filter.SortDescending.GetValueOrDefault();

            if (request.Filter.SortBy == "Id")
            {
                query = sortDescending
                    ? query.OrderByDescending(x => x.Id)
                    : query.OrderBy(x => x.Id);
            }
            else if (request.Filter.SortBy == "Name")
            {
                query = sortDescending
                    ? query.OrderByDescending(x => x.Name)
                    : query.OrderBy(x => x.Name);
            }

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
                IsActive = x.IsActive,
            })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize); ;


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
