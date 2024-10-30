using LevelService.Domain.Enum;
using MediatR;

namespace LevelService.Application.Features.StageFeatures.Commands.Update;

public sealed class UpdateStageCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset DateFrom { get; set; }
    public DateTimeOffset DateTo { get; set; }
    public bool IsExpirable { get; set; }
    public StageStatus Status { get; set; }
}
