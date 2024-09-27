using Microsoft.AspNetCore.Http;
using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.DTOs.Endpoint;

namespace OnAim.Admin.APP.Features.EndpointFeatures.Queries.EndpointsExport;

public record EndpointsExportQuery(
    EndpointFilter Filter,
    List<int>? EndpointIds,
    List<string>? SelectedColumns
    ) : IQuery<IResult>;
