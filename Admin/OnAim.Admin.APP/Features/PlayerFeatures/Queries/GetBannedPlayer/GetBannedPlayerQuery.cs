using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBannedPlayer
{
    public record GetBannedPlayerQuery(int PlayerId) : IQuery<ApplicationResult>;
}
