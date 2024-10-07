using MediatR;

namespace Wheel.Application.Features.ConfigurationFeatures.Queries.GetById;

public class GetConfigurationByIdQuery : IRequest<GetConfigurationByIdQueryResponse>
{
    public int ConfigurationId { get; set; }
}