using GameLib.Domain.Entities;
using GameLib.Domain.Generators;

namespace Wheel.Application.Features.ConfigurationFeatures.Queries.GetById;

public class GetConfigurationByIdQueryResponse
{
    public EntityMetadata Metadata { get; set; }
    public Configuration ConfigurationData { get; set; }
}
