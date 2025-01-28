using OnAim.Admin.Contracts.Helpers;
using OnAim.Admin.Contracts.Dtos.Endpoint;
using System.Dynamic;
using System.Net.Mime;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Contracts.Helpers.Csv;
using OnAim.Admin.APP.CQRS.Query;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Queries.EndpointsExport;

public class EndpointsExportQueryHandler : IQueryHandler<EndpointsExportQuery, IResult>
{
    private readonly IRepository<Domain.Entities.Endpoint> _repository;
    private readonly ICsvWriter<dynamic> _csvWriter;

    public EndpointsExportQueryHandler(
        IRepository<Domain.Entities.Endpoint> repository,
        ICsvWriter<dynamic> csvWriter
        )
    {
        _repository = repository;
        _csvWriter = csvWriter;
    }

    public async Task<IResult> Handle(EndpointsExportQuery request, CancellationToken cancellationToken)
    {
        var query = _repository
             .Query(x =>
                      (string.IsNullOrEmpty(request.Filter.Name) || x.Name.Contains(request.Filter.Name)) &&
                      (!request.Filter.IsActive.HasValue || x.IsActive == request.Filter.IsActive.Value) &&
                      (!request.Filter.Type.HasValue || x.Type == request.Filter.Type.Value)
                  );

        if (request.EndpointIds != null && request.EndpointIds.Any())
            query = query.Where(x => request.EndpointIds.Contains(x.Id));

        if (request.Filter.GroupIds != null && request.Filter.GroupIds.Any())
            query = query.Where(x => x.EndpointGroupEndpoints.Any(ur => request.Filter.GroupIds.Contains(ur.EndpointGroupId)));

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
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var items = await endpoints.ToListAsync();

        var selectedUsers = items.Select(ep =>
        {
            var expandoObj = new ExpandoObject() as IDictionary<string, object>;

            if (request.SelectedColumns == null || !request.SelectedColumns.Any())
            {
                var defaultColumns = new List<string>
                {
                    "Id", "Name", "Description", "IsActive", "DateCreated", "DateUpdated"
                };

                foreach (var column in defaultColumns)
                {
                    var propertyValue = typeof(EndpointResponseModel).GetProperty(column)?.GetValue(ep);
                    expandoObj[column] = propertyValue;
                }
            }
            else
            {
                foreach (var column in request.SelectedColumns)
                {
                    var propertyValue = typeof(EndpointResponseModel).GetProperty(column)?.GetValue(ep);
                    expandoObj[column] = propertyValue;
                }
            }
            return expandoObj;
        }).ToList();

        using var stream = new MemoryStream();
        _csvWriter.Write(selectedUsers, stream);

        return Results.File(stream.ToArray(), MediaTypeNames.Application.Octet, "Permissions.csv");
    }
}
