using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBalance
{
    public record GetPlayerBalanceQuery(int Id) : IQuery<ApplicationResult>;
}
