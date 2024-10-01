using Microsoft.AspNetCore.Http;
using OnAim.Admin.Shared.DTOs.User;
using OnAim.Admin.APP.CQRS.Query;

namespace OnAim.Admin.APP.Feature.UserFeature.Queries.UsersExport;

public record UsersExportQuery(
    UserFilter UserFilter,
    List<int>? UserIds,
    List<string>? SelectedColumns
    ) : IQuery<IResult>;
