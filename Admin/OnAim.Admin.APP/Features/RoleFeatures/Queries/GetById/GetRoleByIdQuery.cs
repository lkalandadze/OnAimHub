﻿using MediatR;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.RoleFeatures.Queries.GetById;

public record GetRoleByIdQuery(int Id) : IQuery<ApplicationResult>;
