using OnAim.Admin.APP.CQRS.Query;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetById;

public record GetGameQuery(string name) : IQuery<object>;
