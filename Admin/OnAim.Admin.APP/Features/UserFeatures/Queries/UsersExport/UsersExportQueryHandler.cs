using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Contracts.Dtos.User;
using System.Net.Mime;
using System.Dynamic;
using OnAim.Admin.Contracts.Helpers.Csv;
using OnAim.Admin.APP.CQRS.Query;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.UsersExport;

public class UsersExportQueryHandler : IQueryHandler<UsersExportQuery, IResult>
{
    private readonly IRepository<Domain.Entities.User> _repository;
    private readonly ICsvWriter<dynamic> _csvWriter;

    public UsersExportQueryHandler(
        IRepository<Domain.Entities.User> repository,
        ICsvWriter<dynamic> csvWriter
        )
    {
        _repository = repository;
        _csvWriter = csvWriter;
    }

    public async Task<IResult> Handle(UsersExportQuery request, CancellationToken cancellationToken)
    {
        var query = _repository
         .Query(x =>
                  (string.IsNullOrEmpty(request.UserFilter.Name) || x.FirstName.Contains(request.UserFilter.Name)) &&
                  (!request.UserFilter.IsActive.HasValue || x.IsActive == request.UserFilter.IsActive.Value) &&
                  (string.IsNullOrEmpty(request.UserFilter.Email) || x.Email.Contains(request.UserFilter.Email)
                  ) &&
                     !x.IsDeleted
              );

        if (request.UserIds != null && request.UserIds.Any())
            query = query.Where(x => request.UserIds.Contains(x.Id));

        if (request.UserFilter.RoleIds != null && request.UserFilter.RoleIds.Any())
            query = query.Where(x => x.UserRoles.Any(ur => request.UserFilter.RoleIds.Contains(ur.RoleId)));

        var totalCount = await query.CountAsync(cancellationToken);

        var pageNumber = request.UserFilter.PageNumber ?? 1;
        var pageSize = request.UserFilter.PageSize ?? 25;

        bool sortDescending = request.UserFilter.SortDescending.GetValueOrDefault();

        if (request.UserFilter.SortBy == "Id" || request.UserFilter.SortBy == "id")
        {
            query = sortDescending
                ? query.OrderByDescending(x => x.Id)
                : query.OrderBy(x => x.Id);
        }
        else if (request.UserFilter.SortBy == "Name" || request.UserFilter.SortBy == "name")
        {
            query = sortDescending
                ? query.OrderByDescending(x => x.FirstName)
                : query.OrderBy(x => x.FirstName);
        }
        else if (request.UserFilter.SortBy == "LastName" || request.UserFilter.SortBy == "lastName")
        {
            query = sortDescending
                ? query.OrderByDescending(x => x.LastName)
                : query.OrderBy(x => x.LastName);
        }

        var users = query
            .Select(x => new ExportUsersModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                IsActive = x.IsActive,
                Phone = x.Phone,
                DateCreated = x.DateCreated,
                Roles = string.Join(", ", x.UserRoles.Select(xx => xx.Role.Name))
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var userList = await users.ToListAsync();

        var selectedUsers = userList.Select(user =>
        {
            var expandoObj = new ExpandoObject() as IDictionary<string, object>;

            if (request.SelectedColumns == null || !request.SelectedColumns.Any())
            {
                var defaultColumns = new List<string>
                {
                    "Id", "FirstName", "LastName", "Email", "DateCreated", "Roles", "IsActive", "Phone", "DateCreated", "DateUpdated"
                };

                foreach (var column in defaultColumns)
                {
                    var propertyValue = typeof(ExportUsersModel).GetProperty(column)?.GetValue(user);
                    expandoObj[column] = propertyValue;
                }
            }
            else
            {
                foreach (var column in request.SelectedColumns)
                {
                    var propertyValue = typeof(ExportUsersModel).GetProperty(column)?.GetValue(user);
                    expandoObj[column] = propertyValue;
                }
            }
            return expandoObj;
        }).ToList();

        using var stream = new MemoryStream();
        _csvWriter.Write(selectedUsers, stream);

        return Results.File(stream.ToArray(), MediaTypeNames.Application.Octet, "Users.csv");
    }
}
