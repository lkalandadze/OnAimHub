using MongoDB.Bson;
using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.CoinFeatures.Queries.GetById;

public record GetCoinByIdQuery(string Id) : IQuery<ApplicationResult>;
