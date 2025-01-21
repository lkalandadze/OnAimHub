using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetGameConfigurations;

public record GetGameConfigurationsQuery(string Name, int? PromotionId, int? ConfigurationId) : IQuery<ApplicationResult>;
