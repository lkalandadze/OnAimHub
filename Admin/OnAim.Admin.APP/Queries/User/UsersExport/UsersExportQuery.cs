using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Models.Request.User;

namespace OnAim.Admin.APP.Queries.User.UsersExport
{
    public record UsersExportQuery(UserFilter UserFilter, List<int> UserIds) : IQuery<FileContentResult>;
}
