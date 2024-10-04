using GameLib.Application.Generators;
using GameLib.Domain.Entities;

namespace Wheel.Application.Features.ConfigurationFeatures.Queries.GetById;

public class GetConfigurationByIdQueryResponse
{
    public EntityMetadata Metadata { get; set; }
    public GameConfiguration ConfigurationData { get; set; }
}
