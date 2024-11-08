using LevelService.Domain.Enum;
using MediatR;

namespace LevelService.Application.Features.StageFeatures.Commands.Create;

public sealed class CreateStageCommand : IRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset DateFrom { get; set; }
    public DateTimeOffset DateTo { get; set; }
    public StageStatus Status { get; set; }
    public bool isExpirable { get; set; }
}
