using MediatR;

namespace LevelService.Application.Features.ConfigurationFeatures.Commands.Delete;

public sealed record DeleteConfigurationCommand : IRequest
{
    public int StageId { get; set; }
    public int ConfigurationId { get; set; }
}
