using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Models.Request.Role;

namespace OnAim.Admin.APP.Queries.Role.RolesExport
{
    public record RolesExportQuery(RoleFilter Filter, List<int> RoleIds) : IQuery<FileContentResult>;
}
