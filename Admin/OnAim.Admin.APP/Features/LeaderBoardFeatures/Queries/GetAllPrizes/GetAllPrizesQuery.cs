using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.LeaderBoardFeatures.Queries.GetAllPrizes;

public record GetAllPrizesQuery : IQuery<ApplicationResult>;

