using LevelService.Application.Models.Levels;
using MediatR;

namespace LevelService.Application.Features.LevelFeatures.Commands.Create;

public class CreateLevelsCommand : IRequest
{
    public int StageId { get; set; }
    public List<LevelModel> Levels { get; set; }
}
