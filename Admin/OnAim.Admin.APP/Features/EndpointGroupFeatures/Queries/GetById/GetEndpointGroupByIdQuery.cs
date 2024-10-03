﻿using MediatR;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.EndpointGroupFeatures.Queries.GetById;

public record GetEndpointGroupByIdQuery(int Id) : IQuery<ApplicationResult>;