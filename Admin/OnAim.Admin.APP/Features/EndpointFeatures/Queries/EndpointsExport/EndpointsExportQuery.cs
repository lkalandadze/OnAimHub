using Microsoft.AspNetCore.Http;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.Dtos.Endpoint;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Queries.EndpointsExport;

public record EndpointsExportQuery(
    EndpointFilter Filter,
    List<int>? EndpointIds,
    List<string>? SelectedColumns
    ) : IQuery<IResult>;
