﻿using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.EndpointGroup;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Queries.GetAll;

public record GetAllEndpointGroupQuery(EndpointGroupFilter Filter) : IQuery<ApplicationResult>;
