﻿using Microsoft.AspNetCore.Http;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.Dtos.EndpointGroup;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Queries.EndpointGroupsExport;

public record EndpointGroupsExportQuery(
    EndpointGroupFilter Filter,
    List<int>? GroupIds,
    List<string>? SelectedColumns
    ) : IQuery<IResult>;
