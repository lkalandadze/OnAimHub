using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Player;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetBalance;

public record GetPlayerBalanceQuery(int Id) : IQuery<ApplicationResult<List<PlayerBalanceDto>>>;
