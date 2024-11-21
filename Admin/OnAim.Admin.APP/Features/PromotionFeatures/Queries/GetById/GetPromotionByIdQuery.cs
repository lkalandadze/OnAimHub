using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Queries.GetById;

public record class GetPromotionByIdQuery(int Id) : IQuery<ApplicationResult>;
