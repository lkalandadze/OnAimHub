using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.APP.Helpers;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Models.Response.EndpointGroup;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Queries.EndpointGroup.EndpointGroupsExport
{
    public class EndpointGroupsExportQueryHandler : IQueryHandler<EndpointGroupsExportQuery, FileContentResult>
    {
        private readonly IRepository<Infrasturcture.Entities.EndpointGroup> _repository;

        public EndpointGroupsExportQueryHandler(IRepository<Infrasturcture.Entities.EndpointGroup> repository)
        {
            _repository = repository;
        }

        public async Task<FileContentResult> Handle(EndpointGroupsExportQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.Query(x =>
                        (string.IsNullOrEmpty(request.Filter.Name) || x.Name.Contains(request.Filter.Name)) &&
                        (!request.Filter.IsActive.HasValue || x.IsActive == request.Filter.IsActive.Value)
               );

            if (request.GroupIds != null && request.GroupIds.Any())
            {
                query = query.Where(x => request.GroupIds.Contains(x.Id));
            }

            if (request.Filter.RoleIds != null && request.Filter.RoleIds.Any())
            {
                query = query.Where(x => x.RoleEndpointGroups.Any(ur => request.Filter.RoleIds.Contains(ur.RoleId)));
            }

            if (request.Filter.EndpointIds != null && request.Filter.EndpointIds.Any())
            {
                query = query.Where(x => x.EndpointGroupEndpoints.Any(ur => request.Filter.EndpointIds.Contains(ur.EndpointId)));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var pageNumber = request.Filter.PageNumber ?? 1;
            var pageSize = request.Filter.PageSize ?? 25;

            bool sortDescending = request.Filter.SortDescending.GetValueOrDefault();

            if (request.Filter.SortBy == "Id" || request.Filter.SortBy == "id")
            {
                query = sortDescending
                    ? query.OrderByDescending(x => x.Id)
                    : query.OrderBy(x => x.Id);
            }
            else if (request.Filter.SortBy == "Name" || request.Filter.SortBy == "name")
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
                    Endpoints = x.EndpointGroupEndpoints.Select(xx => new Infrasturcture.Models.Request.Endpoint.EndpointRequestModel
                    {
                        Name = xx.Endpoint.Name
                    }).ToList(),
                    IsActive = x.IsActive,
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var items = await result.ToListAsync();

            var excelFile = ExcelExportHelper
                .ExportToExcel(items,
                new[] {
                    "Id",
                    "Name",
                    "Endpoints",
                    "IsActive",
                    "DateCreated",
                    "DateUpdated"
                });

            return new FileContentResult(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "PermissionGroups.xlsx"
            };
        }
    }
}
