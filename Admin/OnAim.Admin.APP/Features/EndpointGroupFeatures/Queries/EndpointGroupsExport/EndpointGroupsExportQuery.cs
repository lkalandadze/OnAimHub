using Microsoft.AspNetCore.Http;
using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.DTOs.EndpointGroup;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Queries.EndpointGroupsExport;

public record EndpointGroupsExportQuery(
    EndpointGroupFilter Filter,
    List<int>? GroupIds,
    List<string>? SelectedColumns
    ) : IQuery<IResult>;
