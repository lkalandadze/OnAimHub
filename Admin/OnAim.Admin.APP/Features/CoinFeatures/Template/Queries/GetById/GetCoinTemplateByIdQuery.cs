﻿using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.CoinFeatures.Template.Queries.GetById;

public record GetCoinTemplateByIdQuery(string Id) : IQuery<ApplicationResult>;