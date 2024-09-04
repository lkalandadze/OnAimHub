using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.APP.Helpers;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Extensions;
using OnAim.Admin.Infrasturcture.Models.Response.Endpoint;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Queries.EndPoint.EndpointsExport
{
    public class EndpointsExportQueryHandler : IQueryHandler<EndpointsExportQuery, FileContentResult>
    {
        private readonly IRepository<Endpoint> _repository;

        public EndpointsExportQueryHandler(IRepository<Endpoint> repository)
        {
            _repository = repository;
        }

        public async Task<FileContentResult> Handle(EndpointsExportQuery request, CancellationToken cancellationToken)
        {
            var query = _repository
                 .Query(x =>
                          (string.IsNullOrEmpty(request.Filter.Name) || x.Name.Contains(request.Filter.Name)) &&
                          (!request.Filter.IsActive.HasValue || x.IsActive == request.Filter.IsActive.Value) &&
                          (!request.Filter.Type.HasValue || x.Type == request.Filter.Type.Value) &&
                          (!request.Filter.IsEnable.HasValue || x.IsDeleted == request.Filter.IsEnable.Value)
                      );

            if (request.EndpointIds != null && request.EndpointIds.Any())
            {
                query = query.Where(x => request.EndpointIds.Contains(x.Id));
            }

            if (request.Filter.EndpointGroupIds != null && request.Filter.EndpointGroupIds.Any())
            {
                query = query.Where(x => x.EndpointGroupEndpoints.Any(ur => request.Filter.EndpointGroupIds.Contains(ur.EndpointGroupId)));
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

            var endpoints = query
                .Select(x => new EndpointResponseModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Type = ToHttpMethodExtension.ToHttpMethod(x.Type),
                    IsActive = x.IsActive,
                    IsEnabled = x.IsDeleted,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var items = await endpoints.ToListAsync();

            var excelFile = ExcelExportHelper
                .ExportToExcel(items,
                new[] {
                                "Id",
                                "Name",
                                "IsActive",
                                "DateCreated",
                                "DateUpdated"
                });

            return new FileContentResult(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "Permissions.xlsx"
            };
        }
    }
}
