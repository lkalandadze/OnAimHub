using OnAim.Admin.APP.CQRS.Query;
using OnAim.Admin.Contracts.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetConfigurationMetadata;

public record GetConfigurationMetadataQuery(string Name) : IQuery<ApplicationResult>;
