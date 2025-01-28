using OnAim.Admin.APP.CQRS.Query;

namespace OnAim.Admin.APP.Features.GameFeatures.Queries.GetById.GetConfigurationMetadata;

public record GetConfigurationMetadataQuery(string Name) : IQuery<object>;
