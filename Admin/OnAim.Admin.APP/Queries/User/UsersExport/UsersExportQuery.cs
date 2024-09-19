using Microsoft.AspNetCore.Mvc;
using OnAim.Admin.APP.Queries.Abstract;
using Microsoft.AspNetCore.Http;
using OnAim.Admin.Shared.DTOs.User;

namespace OnAim.Admin.APP.Queries.User.UsersExport
{
    public record UsersExportQuery(
        UserFilter UserFilter,
        List<int>? UserIds,
        List<string>? SelectedColumns
        ) : IQuery<IResult>;
}
