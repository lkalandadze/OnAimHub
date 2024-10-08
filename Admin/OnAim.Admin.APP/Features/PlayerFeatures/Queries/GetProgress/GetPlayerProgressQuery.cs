using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetProgress;

public record GetPlayerProgressQuery(int Id) : IQuery<ApplicationResult>;
