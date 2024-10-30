using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetAllGames;

public record GetAllActiveGamesQuery() : IQuery<ApplicationResult>;

