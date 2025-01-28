using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Player;

namespace OnAim.Admin.APP.Features.PlayerFeatures.Queries.GetById;

public sealed record GetPlayerByIdQuery(int Id) : IQuery<ApplicationResult<PlayerDto>>;
