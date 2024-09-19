using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.DTOs.EndpointGroup;
using Microsoft.AspNetCore.Http;
using OnAim.Admin.Shared.Csv;
using System.Dynamic;
using System.Net.Mime;

namespace OnAim.Admin.APP.Queries.EndpointGroup.EndpointGroupsExport
{
    public class EndpointGroupsExportQueryHandler : IQueryHandler<EndpointGroupsExportQuery, IResult>
    {
        private readonly IRepository<Infrasturcture.Entities.EndpointGroup> _repository;
        private readonly ICsvWriter<dynamic> _csvWriter;

        public EndpointGroupsExportQueryHandler(
            IRepository<Infrasturcture.Entities.EndpointGroup> repository,
             ICsvWriter<dynamic> csvWriter
            )
        {
            _repository = repository;
            _csvWriter = csvWriter;
        }

        public async Task<IResult> Handle(EndpointGroupsExportQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.Query(x =>
                        (string.IsNullOrEmpty(request.Filter.Name) || x.Name.Contains(request.Filter.Name)) &&
                        (!request.Filter.IsActive.HasValue || x.IsActive == request.Filter.IsActive.Value) &&
                         !x.IsDeleted
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
                .Select(x => new ExportEndpointGroupModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    DateUpdated = x.DateUpdated,
                    DateCreated = x.DateCreated,
                    DateDeleted = x.DateDeleted,
                    Endpoints = string.Join(", ", x.EndpointGroupEndpoints.Select(xx => xx.Endpoint.Name)),
                    IsActive = x.IsActive,
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var items = await result.ToListAsync();

            var selectedGroups = items.Select(group =>
            {
                var expandoObj = new ExpandoObject() as IDictionary<string, object>;

                if (request.SelectedColumns == null || !request.SelectedColumns.Any())
                {
                    var defaultColumns = new List<string>
                    {
                        "Id", "Name", "Endpoints", "IsActive", "DateCreated", "DateUpdated"
                    };

                    foreach (var column in defaultColumns)
                    {
                        var propertyValue = typeof(ExportEndpointGroupModel).GetProperty(column)?.GetValue(group);
                        expandoObj[column] = propertyValue;
                    }
                }
                else
                {
                    foreach (var column in request.SelectedColumns)
                    {
                        var propertyValue = typeof(ExportEndpointGroupModel).GetProperty(column)?.GetValue(group);
                        expandoObj[column] = propertyValue;
                    }
                }
                return expandoObj;
            }).ToList();

            using var stream = new MemoryStream();
            _csvWriter.Write(selectedGroups, stream);

            return Results.File(stream.ToArray(), MediaTypeNames.Application.Octet, "PermissionGroups.csv");
        }
    }
}
