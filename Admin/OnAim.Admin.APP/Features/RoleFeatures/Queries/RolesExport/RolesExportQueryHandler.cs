﻿using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Contracts.Dtos.Role;
using Microsoft.AspNetCore.Http;
using System.Dynamic;
using System.Net.Mime;
using OnAim.Admin.Contracts.Helpers.Csv;
using OnAim.Admin.APP.CQRS.Query;

namespace OnAim.Admin.APP.Features.RoleFeatures.Queries.RolesExport;

public class RolesExportQueryHandler : IQueryHandler<RolesExportQuery, IResult>
{
    private readonly IRepository<Domain.Entities.Role> _repository;
    private readonly ICsvWriter<dynamic> _csvWriter;

    public RolesExportQueryHandler(
        IRepository<Domain.Entities.Role> repository,
        ICsvWriter<dynamic> csvWriter
        )
    {
        _repository = repository;
        _csvWriter = csvWriter;
    }
    public async Task<IResult> Handle(RolesExportQuery request, CancellationToken cancellationToken)
    {
        var roleQuery = _repository
           .Query(x =>
                    (string.IsNullOrEmpty(request.Filter.Name) || x.Name.Contains(request.Filter.Name)) &&
                    (!request.Filter.IsActive.HasValue || x.IsActive == request.Filter.IsActive.Value) &&
                     !x.IsDeleted
                );

        if (request.RoleIds != null && request.RoleIds.Any())
            roleQuery = roleQuery.Where(x => request.RoleIds.Contains(x.Id));

        if (request.Filter.UserIds != null && request.Filter.UserIds.Any())
            roleQuery = roleQuery.Where(x => x.UserRoles.Any(ur => request.Filter.UserIds.Contains(ur.UserId)));

        if (request.Filter.GroupIds != null && request.Filter.GroupIds.Any())
            roleQuery = roleQuery.Where(x => x.RoleEndpointGroups.Any(ur => request.Filter.GroupIds.Contains(ur.EndpointGroupId)));

        var totalCount = await roleQuery.CountAsync(cancellationToken);

        var pageNumber = request.Filter.PageNumber ?? 1;
        var pageSize = request.Filter.PageSize ?? 25;

        bool sortDescending = request.Filter.SortDescending.GetValueOrDefault();

        if (request.Filter.SortBy == "Id" || request.Filter.SortBy == "id")
        {
            roleQuery = sortDescending
                ? roleQuery.OrderByDescending(x => x.Id)
                : roleQuery.OrderBy(x => x.Id);
        }
        else if (request.Filter.SortBy == "Name" || request.Filter.SortBy == "name")
        {
            roleQuery = sortDescending
                ? roleQuery.OrderByDescending(x => x.Name)
                : roleQuery.OrderBy(x => x.Name);
        }

        var roleResult = roleQuery
            .Select(x => new ExportRoleShortResponseModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive,
                EndpointGroups = string.Join(", ", x.RoleEndpointGroups.Select(xx => xx.EndpointGroup.Name)),
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var items = await roleResult.ToListAsync(cancellationToken);

        var selectedRoles = items.Select(role =>
        {
            var expandoObj = new ExpandoObject() as IDictionary<string, object>;

            if (request.SelectedColumns == null || !request.SelectedColumns.Any())
            {
                var defaultColumns = new List<string>
                {
                    "Id", "Name", "Description", "EndpointGroupModels", "IsActive", "DateCreated", "DateUpdated"
                };

                foreach (var column in defaultColumns)
                {
                    var propertyValue = typeof(ExportRoleShortResponseModel).GetProperty(column)?.GetValue(role);
                    expandoObj[column] = propertyValue;
                }
            }
            else
            {
                foreach (var column in request.SelectedColumns)
                {
                    var propertyValue = typeof(ExportRoleShortResponseModel).GetProperty(column)?.GetValue(role);
                    expandoObj[column] = propertyValue;
                }
            }
            return expandoObj;
        }).ToList();

        using var stream = new MemoryStream();
        _csvWriter.Write(selectedRoles, stream);

        return Results.File(stream.ToArray(), MediaTypeNames.Application.Octet, "Roles.csv");
    }
}
