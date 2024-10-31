using LevelService.Application.Features.ConfigurationFeatures.Helper;
using MediatR;

namespace LevelService.Application.Features.ConfigurationFeatures.Commands.Create;

public class CreateConfigurationCommand : IRequest
{
    public int StageId { get; set; }
    public List<CreateConfigurationModel> Configurations { get; set; }
}
