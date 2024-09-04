using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.APP.Helpers;
using OnAim.Admin.APP.Models.Response.User;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Repository.Abstract;

namespace OnAim.Admin.APP.Queries.User.UsersExport
{
    public class UsersExportQueryHandler : IQueryHandler<UsersExportQuery, FileContentResult>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _repository;

        public UsersExportQueryHandler(IRepository<Infrasturcture.Entities.User> repository)
        {
            _repository = repository;
        }

        public async Task<FileContentResult> Handle(UsersExportQuery request, CancellationToken cancellationToken)
        {
            var query = _repository
             .Query(x =>
                      (string.IsNullOrEmpty(request.UserFilter.Name) || x.FirstName.Contains(request.UserFilter.Name)) &&
                      (!request.UserFilter.IsActive.HasValue || x.IsActive == request.UserFilter.IsActive.Value)
                      && (string.IsNullOrEmpty(request.UserFilter.Email) || x.Email.Contains(request.UserFilter.Email)
                      )
                  );

            if (request.UserIds != null && request.UserIds.Any())
            {
                query = query.Where(x => request.UserIds.Contains(x.Id));
            }

            if (request.UserFilter.RoleIds != null && request.UserFilter.RoleIds.Any())
            {
                query = query.Where(x => x.UserRoles.Any(ur => request.UserFilter.RoleIds.Contains(ur.RoleId)));
            }

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
                .Select(x => new UsersModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    IsActive = x.IsActive,
                    Phone = x.Phone,
                    DateCreated = x.DateCreated,
                    Roles = x.UserRoles.Select(xx => new RoleDto
                    {
                        Name = xx.Role.Name,
                    }).ToList(),
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var userList = await users.ToListAsync();

            var excelFile = ExcelExportHelper
                .ExportToExcel(userList,
                new[] {
                    "Id",
                    "FirstName",
                    "LastName",
                    "Email",
                    "Roles",
                    "IsActive",
                    "Phone",
                    "DateCreated",
                    "DateUpdated"
                });

            return new FileContentResult(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "Users.xlsx"
            };
        }
    }
}
