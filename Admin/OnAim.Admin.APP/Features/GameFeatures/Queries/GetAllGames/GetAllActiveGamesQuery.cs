using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.APP.Services.Game;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Game;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetAllGames;

public record GetAllActiveGamesQuery(FilterGamesDto Filter) : IQuery<ApplicationResult>;

