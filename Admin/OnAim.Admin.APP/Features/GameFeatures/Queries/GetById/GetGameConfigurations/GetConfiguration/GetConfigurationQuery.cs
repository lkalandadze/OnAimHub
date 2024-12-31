using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetGameConfigurations.GetConfiguration;

public record GetConfigurationQuery(string Name, int Id) : IQuery<ApplicationResult>;
