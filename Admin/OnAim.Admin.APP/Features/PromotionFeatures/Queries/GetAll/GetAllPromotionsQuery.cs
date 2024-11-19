using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Promotion;

namespace OnAim.Admin.APP.Features.PromotionFeatures.Queries.GetAll;

public record GetAllPromotionsQuery(PromotionFilter Filter) : IQuery<ApplicationResult>;
