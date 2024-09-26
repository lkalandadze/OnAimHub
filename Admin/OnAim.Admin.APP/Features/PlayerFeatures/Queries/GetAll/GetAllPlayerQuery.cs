using OnAim.Admin.APP.CQRS;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Player;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetAll;

public sealed record GetAllPlayerQuery(
   PlayerFilter? Filter
    ) : IQuery<ApplicationResult>;
