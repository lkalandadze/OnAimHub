using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetById;

public sealed record GetPlayerByIdQuery(int Id) : IQuery<ApplicationResult>;
